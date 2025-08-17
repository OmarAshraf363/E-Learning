using Banha_UniverCity.Repository.IRepository;
using BFCAI.Models;
using BFCAI.Models.ViewModels;
using BFCAI.Utility.Helper;
using DataAccess.Repository.IRepository.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.ModelsRepository.Service
{
    public class CartService: ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
         
        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CartVM> GetCartViewModelAsync(string userId)
        {
            var model = new CartVM();
            var cart = await _unitOfWork.cartRepository.GetOneAsync(e => e.UserId == userId);
            if (cart == null) return model;

            var cartItems = await _unitOfWork.cartItemRepository.GetAllAsync(e => e.CartId == cart.Id,includes: e => e.Course);
            var courseIds = cartItems.Select(e => e.CourseId).ToList();
            var keywords =  _unitOfWork.keyWordRepository
                .Get(e => courseIds.Contains(e.CourseId))
                .Select(e => e.Name.ToLower()).ToList();

            var recommendedIds = _unitOfWork.keyWordRepository
                .Get(e => keywords.Contains(e.Name.ToLower()) && !courseIds.Contains(e.CourseId))
                .Select(e => e.CourseId).Distinct().ToList();

            var recommendedCourses = _unitOfWork.courseRepository.Get(e => recommendedIds.Contains(e.CourseID)).PrepareCoursesToViewDetailAsync();

            model.CartItems = cartItems.ToList();
            model.Courses = await recommendedCourses.ToListAsync();

            return model;
        }
        public async Task AddToCartAsync(string userId, int courseId, int quantity)
        {
            var cart = await _unitOfWork.cartRepository.GetOneAsync(e => e.UserId == userId) ?? new Cart { UserId = userId };
            if (cart.Id == 0) await _unitOfWork.cartRepository.AddAsync(cart);
            _unitOfWork.Commit();

            var course = await _unitOfWork.courseRepository.GetOneAsync(e => e.CourseID == courseId);

            var cartItem = await _unitOfWork.cartItemRepository.GetOneAsync(e => e.CourseId == courseId && e.CartId == cart.Id);
            if (cartItem == null)
            {
                cartItem = new CartItems { CartId = cart.Id, CourseId = courseId, Quantity = quantity, Price = course?.Price };
                await _unitOfWork.cartItemRepository.AddAsync(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                cartItem.Price += course?.Price;
            }

            _unitOfWork.Commit();
        }
        public async Task RemoveFromCartAsync(int cartItemId)
        {
            var cartItem = await _unitOfWork.cartItemRepository.GetOneAsync(e => e.Id == cartItemId);
            if (cartItem != null)
            {
                await _unitOfWork.cartItemRepository.DeleteAsync(cartItem);
                _unitOfWork.Commit();
            }
        }
        public async Task<Cart?> GetUserCart(string userId)
        {
            return await _unitOfWork.cartRepository.GetOneAsync(e => e.UserId == userId);
        }
    }
}
