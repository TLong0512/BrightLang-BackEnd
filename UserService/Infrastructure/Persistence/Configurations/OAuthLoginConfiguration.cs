using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class OAuthLoginConfiguration : IEntityTypeConfiguration<OAuthLogin>
{
    public void Configure(EntityTypeBuilder<OAuthLogin> builder)
    {
        BaseEntityConfiguration.Configure(builder);

        builder.ToTable(nameof(OAuthLogin));

        builder.HasKey(oal => new { oal.LoginProvider, oal.ProviderKey });

        builder.Property(oal => oal.LoginProvider)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(oal => oal.ProviderKey)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(oal => oal.ProviderDisplayName)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(oal => oal.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        // khi một người dùng bị xoá, các phương thức đăng nhập
        // bằng bên thứ ba (như thông qua Google) cũng bị xoá theo.
    }
}
