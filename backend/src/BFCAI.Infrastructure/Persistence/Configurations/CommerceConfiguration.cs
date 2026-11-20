using BFCAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFCAI.Infrastructure.Persistence.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasOne(c => c.ApplicationUser).WithMany(u => u.Carts)
            .HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Items).WithOne(ci => ci.Cart)
            .HasForeignKey(ci => ci.CartId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class CartItemsConfiguration : IEntityTypeConfiguration<CartItems>
{
    public void Configure(EntityTypeBuilder<CartItems> builder)
    {
        builder.HasOne(ci => ci.Course).WithMany()
            .HasForeignKey(ci => ci.CourseId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasOne(o => o.ApplicationUser).WithMany(u => u.Orders)
            .HasForeignKey(o => o.AppUserId).OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.OrderItems).WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class OrderItemsConfiguration : IEntityTypeConfiguration<OrderItems>
{
    public void Configure(EntityTypeBuilder<OrderItems> builder)
    {
        builder.HasOne(oi => oi.Course).WithMany()
            .HasForeignKey(oi => oi.CourseId).OnDelete(DeleteBehavior.Restrict);
    }
}
