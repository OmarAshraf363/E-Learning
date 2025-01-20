using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using BFCAI.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Banha_UniverCity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string filter)
        {
            var applicationUsers=new List<ApplicationUser>();
            var usersFromIdentity=_userManager.Users.ToList();
            foreach (var user in usersFromIdentity)
            {
                applicationUsers.Add(user as ApplicationUser);
            }
            if (!string.IsNullOrEmpty(filter))
            {
                applicationUsers=applicationUsers.Where(e => e.UserType == filter).ToList();
            }
            return View(applicationUsers);
        }


        public async Task<IActionResult> LockUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Lock out the user until a specific date
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
            var appUser = user as ApplicationUser;
            return RedirectToAction(nameof(Index), new {filter= appUser?.UserType });
        }
        public async Task<IActionResult> UnlockUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Unlock the user
            await _userManager.SetLockoutEndDateAsync(user, null);

            var appUser = user as ApplicationUser;
            return RedirectToAction(nameof(Index), new { filter = appUser?.UserType });
        }


        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
            }

            return RedirectToAction(nameof(Index));
        }


            // GET: Upsert (for Create or Edit)
            public async Task<IActionResult> Upsert(string id)
            {
                if (id == null)
                {
                    // Creating a new user
                    return View(new UserViewModel());
                }
                else
                {
                    // Editing an existing user
                    var user = await _userManager.FindByIdAsync(id);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    var userViewModel = new UserViewModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? string.Empty
                    };

                    return View(userViewModel);
                }
            }

            // POST: Upsert (for saving Create or Edit)
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Upsert(UserViewModel model)
            {
                if (ModelState.IsValid)
                {
                    if (model.Id == null)
                    {
                        // Create new user
                        var user = new ApplicationUser
                        {
                            UserName = model.UserName,
                            Email = model.Email
                        };

                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            if (!string.IsNullOrEmpty(model.Role))
                            {
                                await _userManager.AddToRoleAsync(user, model.Role);
                            }
                        
                        return RedirectToAction(nameof(Index), new { filter = user?.UserType });
                    }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                    else
                    {
                        // Update existing user
                        var user = await _userManager.FindByIdAsync(model.Id);
                        if (user == null)
                        {
                            return NotFound();
                        }

                        user.UserName = model.UserName;
                        user.Email = model.Email;

                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            var currentRoles = await _userManager.GetRolesAsync(user);
                            if (!currentRoles.Contains(model.Role))
                            {
                                await _userManager.RemoveFromRolesAsync(user, currentRoles.ToArray());
                                await _userManager.AddToRoleAsync(user, model.Role);
                            }
                            var appUser=user as ApplicationUser;
                        return RedirectToAction(nameof(Index), new { filter = appUser?.UserType });
                    }

                    foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

                return View(model);
            }
        }

    }

