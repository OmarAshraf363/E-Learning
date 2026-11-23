using BFCAI.Application.Common;
using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Application.Features.Catalog;
using BFCAI.Domain.Entities;
using CartEntity = BFCAI.Domain.Entities.Cart;

namespace BFCAI.Application.Features.Cart;

/// <summary>Current user's shopping cart (legacy CartController + CartService).</summary>
public interface ICartService
{
    Task<CartDto> GetAsync();
    Task<CartSummaryDto> GetSummaryAsync();
    Task<CartDto> AddAsync(AddToCartRequest request);
    Task<CartDto> RemoveAsync(int cartItemId);
}

public class CartService(IUnitOfWork unitOfWork, ICurrentUser currentUser) : ICartService
{
    private IGenericRepository<CartItems> Items => unitOfWork.Repository<CartItems>();

    public async Task<CartDto> GetAsync()
    {
        var userId = currentUser.RequiredUserId;

        var items = await Items.SelectAsync(
            i => new CartItemDto(i.Id, i.CourseId!.Value, i.Course!.CourseName, FilePaths.Cover(i.Course.ImgCover),
                i.Course.Instructor != null ? i.Course.Instructor.FullName : null, i.Price ?? 0, i.Quantity ?? 1),
            i => i.Cart!.UserId == userId && i.CourseId != null);

        // Legacy: recommend other courses that share a keyword with the cart courses.
        var courseIds = items.Select(i => i.CourseId).ToList();
        var keywords = await unitOfWork.Repository<KeyWord>().SelectAsync(
            k => k.Name.ToLower(), k => k.CourseId != null && courseIds.Contains(k.CourseId.Value));
        var recommendations = keywords.Count == 0
            ? []
            : await unitOfWork.Repository<Course>().SelectAsync(CourseCardDto.Projection,
                c => !courseIds.Contains(c.CourseID) && c.Keywords.Any(k => keywords.Contains(k.Name.ToLower())));

        return new CartDto(items, items.Count, items.Sum(i => i.Price * i.Quantity), recommendations);
    }

    /// <summary>
    /// Navbar cart badge (legacy CartIcon view component) + the course ids the course cards need
    /// to show "Add to Cart", "Go to Cart" or "Open course".
    /// </summary>
    public async Task<CartSummaryDto> GetSummaryAsync()
    {
        var userId = currentUser.RequiredUserId;
        var cartCourseIds = await Items.SelectAsync(i => i.CourseId!.Value, i => i.Cart!.UserId == userId && i.CourseId != null);
        return new CartSummaryDto(cartCourseIds.Count, cartCourseIds, await EnrolledCourseIdsAsync(unitOfWork, userId));
    }

    /// <summary>Courses the user is already enrolled in (they can't be bought again).</summary>
    internal static Task<List<int>> EnrolledCourseIdsAsync(IUnitOfWork unitOfWork, string userId)
        => unitOfWork.Repository<Enrollment>().SelectAsync(e => e.CourseID!.Value, e => e.StudentId == userId && e.CourseID != null);

    /// <summary>
    /// Adds the course to the cart. Business rule (second plan): a course can be in the cart only once
    /// and can't be bought by a user who is already enrolled in it, so the quantity is always 1.
    /// </summary>
    public async Task<CartDto> AddAsync(AddToCartRequest request)
    {
        var userId = currentUser.RequiredUserId;
        var course = await unitOfWork.Repository<Course>().GetOneAsync(c => c.CourseID == request.CourseId)
                     ?? throw new NotFoundException(nameof(Course), request.CourseId);

        if (await unitOfWork.Repository<Enrollment>().AnyAsync(e => e.StudentId == userId && e.CourseID == course.CourseID))
            throw new BadRequestException("You are already enrolled in this course.");

        var cart = await unitOfWork.Repository<CartEntity>().GetOneAsync(c => c.UserId == userId, tracked: true, c => c.Items);
        if (cart is null)
        {
            cart = new CartEntity { UserId = userId };
            await unitOfWork.Repository<CartEntity>().AddAsync(cart);
        }

        if (cart.Items.Any(i => i.CourseId == course.CourseID))
            throw new BadRequestException("This course is already in your cart.");

        cart.Items.Add(new CartItems { CourseId = course.CourseID, Quantity = 1, Price = course.Price });
        await unitOfWork.SaveChangesAsync();
        return await GetAsync();
    }

    /// <summary>Technical fix: legacy deleted any cart item by id, even another user's.</summary>
    public async Task<CartDto> RemoveAsync(int cartItemId)
    {
        var userId = currentUser.RequiredUserId;
        var item = await Items.GetOneAsync(i => i.Id == cartItemId && i.Cart!.UserId == userId, tracked: true)
                   ?? throw new NotFoundException("Cart item", cartItemId);

        Items.Delete(item);
        await unitOfWork.SaveChangesAsync();
        return await GetAsync();
    }
}
