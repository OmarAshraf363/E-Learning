using BFCAI.Application.Features.Catalog;

namespace BFCAI.Application.Features.Cart;

/// <summary>Price is the course unit price; the line total is Price x Quantity.</summary>
public record CartItemDto(int Id, int CourseId, string CourseName, string? Cover, string? InstructorName, decimal Price, int Quantity);

/// <summary>Legacy CartVM: items + "You might also like" (courses sharing keywords).</summary>
public record CartDto(List<CartItemDto> Items, int TotalCourses, decimal Subtotal, List<CourseCardDto> Recommendations);

public record AddToCartRequest(int CourseId);

/// <summary>Navbar badge count + the course ids used by the course cards' buttons.</summary>
public record CartSummaryDto(int Count, List<int> CartCourseIds, List<int> EnrolledCourseIds);

public record CheckoutStartDto(string Url);

/// <summary>Shown on the payment success page.</summary>
public record CheckoutResultDto(int OrderId, decimal Total, DateTime OrderDate);
