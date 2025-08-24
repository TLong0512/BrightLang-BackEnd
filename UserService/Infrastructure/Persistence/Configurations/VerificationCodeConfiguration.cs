using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class VerificationCodeConfiguration : IEntityTypeConfiguration<VerificationCode>
{
    public void Configure(EntityTypeBuilder<VerificationCode> builder)
    {
        BaseEntityConfiguration.Configure(builder);

        builder.ToTable(nameof(VerificationCode));

        builder.HasKey(vc => vc.Id);
        builder.Property(vc => vc.Id)
            .ValueGeneratedOnAdd();

        builder.Property(vc => vc.Email)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasIndex(vc => vc.Email)
            .IsUnique();

        builder.Property(vc => vc.Code)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(vc => vc.ExpireAt)
            .IsRequired();
    }
}
