using BFCAI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFCAI.Infrastructure.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasMany(p => p.Comments).WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId).OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(p => p.KeyWords).WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId).OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(p => p.Reactions).WithOne(e => e.Post)
            .HasForeignKey(r => r.PostId).OnDelete(DeleteBehavior.NoAction);
    }
}

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasOne(c => c.ParentComment).WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentCommentId);

        builder.HasMany(c => c.Reactions).WithOne(e => e.Comment)
            .HasForeignKey(r => r.CommentId).OnDelete(DeleteBehavior.NoAction);
    }
}

public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
{
    public void Configure(EntityTypeBuilder<Reaction> builder)
    {
        builder.Property(r => r.Type).IsRequired();
    }
}

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.Property(t => t.UserId).HasMaxLength(450);
        builder.Property(t => t.TokenHash).HasMaxLength(128);
        builder.HasIndex(t => t.TokenHash).IsUnique();
        builder.Ignore(t => t.IsActive);
        builder.HasOne<IdentityUser>().WithMany()
            .HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
