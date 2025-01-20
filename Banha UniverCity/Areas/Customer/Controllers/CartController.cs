using Banha_UniverCity.Repository.IRepository;
using BFCAI.Models;
using BFCAI.Models.ViewModels;
using DataAccess.Repository.ModelsRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stripe.Checkout;

namespace Banha_UniverCity.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {

            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new CartVM();
            var userCart = _unitOfWork.cartRepository.GetOne(e => e.UserId == _userManager.GetUserId(User));
            if(userCart == null) { return View(model); }
            var cartItems=_unitOfWork.cartItemRepository.Get(e=>e.CartId==userCart.Id,e=>e.Course);
            var ides=cartItems.Select(e=>e.CourseId).ToList();
            var keyWords=_unitOfWork.keyWordRepository.Get(e=>ides.Contains(e.CourseId)).Select(e=>e.Name.ToLower()).ToList();
            var coursesIDS=_unitOfWork.keyWordRepository.Get(e=>keyWords.Contains(e.Name.ToLower())&&!ides.Contains(e.CourseId))
                .Select(e=>e.CourseId)
                .Distinct()
                .ToList();
            var courses = _unitOfWork.courseRepository.Get(e => coursesIDS.Contains(e.CourseID));

            model.CartItems = cartItems.ToList();
            model.Courses = courses.ToList();
           


            return View(model);
        }
        public IActionResult AddToCart(int?courseId,int?quantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            //getUser
            var userId = _userManager.GetUserId(User);
            var user=_userManager.Users.FirstOrDefault(e=>e.Id== userId);
            //getSelected Course
            var course=_unitOfWork.courseRepository.GetOne(e=>e.CourseID== courseId);
            //get cart if found , if not fount create new one 
            var cart =_unitOfWork.cartRepository.GetOne(e=>e.UserId== userId);
            if (cart == null) {
                cart = new Cart()
                {
                    UserId = userId,
                };
                _unitOfWork.cartRepository.Create(cart);
                _unitOfWork.Commit();
            }
            var order = _unitOfWork.orderRepository.GetOne(e => e.AppUserId == userId&&e.OrderStatus==0);
            if (order == null)
            {
                order = new Order()
                {
                    AppUserId = userId,
                    OrderDate = DateTime.Now,
                    OrderStatus = 0 
                };
                _unitOfWork.orderRepository.Create(order); 
                _unitOfWork.Commit();
            }
            var orderItem=_unitOfWork.orderItemRepository.GetOne(e=>e.CourseId==courseId);
            if (orderItem == null) 
            {
                orderItem = new OrderItems()
                {
                    OrderId=order.Id,
                    CourseId = courseId,
                    Quantity = quantity,
                    ListPrice=course?.Price,
                    TotalPrice=quantity*course?.Price

                } ;
             
                _unitOfWork.orderItemRepository.Create(orderItem);
                _unitOfWork.Commit();


            }
            else
            {
                orderItem.Quantity += quantity;
                orderItem.TotalPrice += quantity * course?.Price;
            }
            var cartItem=_unitOfWork.cartItemRepository.GetOne(e=>e.CourseId == courseId&&e.CartId==cart.Id);
            if (cartItem == null) {
                cartItem = new CartItems()
                {
                    CartId=cart.Id,
                    CourseId=courseId,
                    Quantity = quantity,
                    Price=course?.Price,
                };
                _unitOfWork.cartItemRepository.Create(cartItem);
                _unitOfWork.Commit();
            }
            else
            {
                cartItem.Quantity += quantity;
                cartItem.Price += course?.Price;
            }
            return RedirectToAction("Index");
        }
        public IActionResult RemoveFromCart(int id)
        {
            var cartItem = _unitOfWork.cartItemRepository.GetOne(e => e.Id == id);
            
            if (cartItem == null) { return NotFound(); }
            var orderItem=_unitOfWork.orderItemRepository.GetOne(e=>e.CourseId == cartItem.CourseId);
            if (orderItem == null) { return NotFound(); }
            _unitOfWork.cartItemRepository.Delete(cartItem);
            _unitOfWork.orderItemRepository.Delete(orderItem);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }


        public IActionResult Pay()
        {

            var userCart = _unitOfWork.cartRepository.GetOne(e => e.UserId == _userManager.GetUserId(User));
            if (userCart == null) { return NotFound(); }
            var cartItems = _unitOfWork.cartItemRepository.Get(e => e.CartId == userCart.Id, e => e.Course);
            var order = _unitOfWork.orderRepository.GetOne(e => e.AppUserId == _userManager.GetUserId(User)&&e.OrderStatus==0);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),

                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/{StaticData.role_Customer}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/{StaticData.role_Customer}/checkout/cancel",
            };
            foreach (var model in cartItems)
            {
                var result = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = model.Course?.CourseName,
                           Description=model.Course?.Description,
                           

                        },
                        UnitAmount = (long)model.Course?.Price * 100,
                    },
                    Quantity = model.Quantity,
                };
                options.LineItems.Add(result);
            }
            var service = new SessionService();
            var session = service.Create(options);

            if (order != null)
            {
                order.StripeChargeId = session.Id; // Save the session ID or charge ID as needed
                _unitOfWork.Commit();
            }
            return Redirect(session.Url);

        }


        //public IActionResult Pay()
        //{
        //    var items = JsonConvert.DeserializeObject<IEnumerable<CartViewModel>>((string)TempData["shoppingCart"]);
        //    var order = unitOfWork.OrderRepository.Get(e => e.OrderId == items.FirstOrDefault().OrderId)?.FirstOrDefault();
        //    var options = new SessionCreateOptions
        //    {
        //        PaymentMethodTypes = new List<string> { "card" },
        //        LineItems = new List<SessionLineItemOptions>(),

        //        Mode = "payment",
        //        SuccessUrl = $"{Request.Scheme}://{Request.Host}/{Methods.StaticData_CustomerRole}/checkout/success",
        //        CancelUrl = $"{Request.Scheme}://{Request.Host}/{Methods.StaticData_CustomerRole}/checkout/cancel",
        //    };
        //    foreach (var model in items)
        //    {
        //        var result = new SessionLineItemOptions
        //        {
        //            PriceData = new SessionLineItemPriceDataOptions
        //            {
        //                Currency = "usd",
        //                ProductData = new SessionLineItemPriceDataProductDataOptions
        //                {
        //                    Name = model.ProductName,
        //                },
        //                UnitAmount = (long)model.ListPrice * 100,
        //            },
        //            Quantity = model.Quantity,
        //        };
        //        options.LineItems.Add(result);
        //    }
        //    var service = new SessionService();
        //    var session = service.Create(options);

        //    if (order != null)
        //    {
        //        order.StripeChargeId = session.Id; // Save the session ID or charge ID as needed
        //        unitOfWork.OrderRepository.Save();
        //    }
        //    return Redirect(session.Url);
        //}
    }
}
