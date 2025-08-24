using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        BaseEntityConfiguration.Configure(builder);

        builder.ToTable(nameof(RefreshToken));

        builder.HasKey(ut => ut.Id);
        builder.Property(ut => ut.Id)
            .ValueGeneratedOnAdd();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        // khi 1 người dùng bị xoá, mọi token cũng theo đó mà bị xoá theo.

        builder.Property(ut => ut.Token)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(ut => ut.ExpireAt)
            .IsRequired();

        builder.Property(ut => ut.RevokeAt)
            .IsRequired(false);
    }
}
