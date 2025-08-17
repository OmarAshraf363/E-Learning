using BFCAI.Models;
using BFCAI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository.Service
{
    public interface ICartService
    {
        Task<CartVM> GetCartViewModelAsync(string userId);
        Task AddToCartAsync(string userId, int courseId, int quantity);
        Task RemoveFromCartAsync(int cartItemId);
        Task<Cart?> GetUserCart(string userId);
    }
}
