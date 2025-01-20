using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using BFCAI.Models;
using BFCAI.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Net.Mail;

namespace Banha_UniverCity.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CommunityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public CommunityController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index(int? id ,string?word)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var communties = id.HasValue ?
                _unitOfWork.communityRepository.Get(e => e.Id == id).ToList()
                : _unitOfWork.communityRepository.Get().ToList();

            var postsQuery = _unitOfWork.postRepository.Get(
                      null,
                  e => e.Comments,
                  e => e.User,
                      e => e.Community,
                  e => e.Reactions
            );

            if (id.HasValue)
            {
                postsQuery = postsQuery.Where(e => e.CommunityId == id);
            }
            var model = new CommunityVM
            {
                Communities = communties,
                Posts = postsQuery.ToList(),
                Reactions = _unitOfWork.reactionRepository.Get().ToList(),
            };
            if (id.HasValue)
            {
                model.Community = _unitOfWork.communityRepository.GetOne(e => e.Id == id);
            }
            // Add user-specific data
            var currentUserId = _userManager.GetUserId(User);

            model.ApplicationUser = _userManager.Users.FirstOrDefault(e => e.Id == currentUserId) as ApplicationUser;
            return View(model);
        }
        public IActionResult UpsertPost(int? id, int? bindId)
        {
            //bindId=>Communityu Id if Sended
            Post post = new Post();
            if (id == null || id == 0)
            {
                post.UserId = _userManager.GetUserId(User);
                post.CreatedAt = DateTime.Now;
                if (bindId != null) { post.CommunityId = bindId; }
                return PartialView(post);
            }
            post = _unitOfWork.postRepository.GetOne(e => e.Id == id,e=>e.KeyWords);
            if (post == null)
            {
                return NotFound();
            }
          
            return PartialView(post);

        }
        [HttpPost]
        public IActionResult UpsertPost(Post post, IFormFile? Attachment)
        {
            if (ModelState.IsValid)
            {
                var oldPostImagePath = _unitOfWork.postRepository.GetOneWithNoTrack(e => e.Id == post.Id)?.Attachment;

                if (Attachment != null)
                {
                    if (post.Id!=0)
                    {
                
                        if (oldPostImagePath != null)
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldPostImagePath);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                try
                                {
                                    System.IO.File.Delete(oldFilePath);
                                }
                                catch (Exception ex)
                                {
                                    ModelState.AddModelError("Attachment", "An error occurred while deleting the old file.");
                                    return StaticData.CheckValidation(ModelState, Request, false);

                                }
                            }
                        }
                    }
                    // Validate the file (optional, e.g., size, type)
                    if (Attachment.Length > 0)
                    {
                        // Get the file extension
                        string fileExtension = Path.GetExtension(Attachment.FileName);

                        // Generate a unique file name to avoid overwriting
                        string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

                        // Define the file path
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "attachments");

                        // Ensure the directory exists
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Combine the folder path with the unique file name
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Save the file to the server
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            Attachment.CopyTo(fileStream);
                        }

                        // Save the file path in the Post object (relative to the `wwwroot` folder)
                        post.Attachment = Path.Combine("uploads", "attachments", uniqueFileName);
                    }
                }
                if (Attachment == null && post.Id != 0) {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldPostImagePath);

                    if (System.IO.File.Exists(oldFilePath))
                    {
                        try
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("Attachment", "An error occurred while deleting the old file.");
                            return StaticData.CheckValidation(ModelState, Request, false);

                        }
                    }
                }
                if (post.Id == 0)
                {
                    _unitOfWork.postRepository.Create(post);
                   
                    
                  
                }
                else
                {
                    var oldWords = _unitOfWork.keyWordRepository.Get(e => e.PostId == post.Id);
                    _unitOfWork.keyWordRepository.DeleteRange(oldWords);
               
                    _unitOfWork.postRepository.Edit(post);
            
                }

                _unitOfWork.Commit();
                return StaticData.CheckValidation(ModelState, Request, true);
            }

            return StaticData.CheckValidation(ModelState, Request, false);
        }



        public IActionResult UpsertComment(int? id, int bindId)
        {
            //bindId=>PostId
            Comment comment = new Comment();
            if (id == null||id==0)
            {
                comment.UserId = _userManager.GetUserId(User);
                comment.CreatedAt = DateTime.Now;
                comment.PostId = bindId;
                return PartialView(comment);
            }
            comment = _unitOfWork.commentRepository.GetOne(e => e.Id == id);
            if (comment == null)
            {
                return NotFound();
            }
            return PartialView(comment);

        }
        [HttpPost]
        public IActionResult UpsertComment(Comment comment)
        {
            JsonResult result;
            if (ModelState.IsValid)
            {
                if (comment.Id == 0)
                {
                   
                    _unitOfWork.commentRepository.Create(comment);
                    _unitOfWork.Commit();
                    return StaticData.CheckValidation(ModelState, Request, true);
                }
                _unitOfWork.commentRepository.Edit(comment);
                _unitOfWork.Commit();

                return StaticData.CheckValidation(ModelState, Request, true);

            }
            return StaticData.CheckValidation(ModelState, Request, false);

        }

        [HttpPost]
        public IActionResult AddReaction(int id, string typeReact, string targetType)
        {
            if (ModelState.IsValid)
            {
                var react = _unitOfWork.reactionRepository.GetOne(e => e.PostId == id
                &&
                e.UserId == _userManager.GetUserId(User));
                if (react == null)
                {

                    Reaction reaction = new Reaction()
                    {
                        PostId = id,
                       
                        Type = typeReact,
                        UserId = _userManager.GetUserId(User)

                    };
                    _unitOfWork.reactionRepository.Create(reaction);
                    _unitOfWork.Commit();

                    return Json(new { state = "Add", typeReact = typeReact, valid = true });

                }
                _unitOfWork.reactionRepository.Delete(react);
                _unitOfWork.Commit();
                return Json(new { state = "Remove", valid = true });


            }
            return Json(new { valid = false });
        }


        public IActionResult GetReaction(int id)
        {
            var postReaction = _unitOfWork.reactionRepository.Get(e => e.PostId == id , e => e.User);
            return PartialView(postReaction);
        }
        public IActionResult GetComments(int id)
        {
            var comments = _unitOfWork.commentRepository.Get(e => e.PostId == id, e => e.User);
            return PartialView(comments);
        }
        [HttpGet]
        public IActionResult CountReaction(int id)
        {
            var count = _unitOfWork.reactionRepository.Get(e => e.PostId == id).Count();
            return Json(new { count = count });
        }

        public IActionResult DeletePost(int id,int? comId) 
        {
            var post=_unitOfWork.postRepository.GetOne(e=>e.Id == id);
            if(post != null) 
            {
                _unitOfWork.postRepository.Delete(post);
                var reactions=_unitOfWork.reactionRepository.Get(e=>e.PostId == id);
                _unitOfWork.reactionRepository.DeleteRange(reactions);
                _unitOfWork.Commit();
                if (comId == 0||comId==null)
                {

                return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new {id=comId});

            }
            return NotFound();
        }

    }
}
