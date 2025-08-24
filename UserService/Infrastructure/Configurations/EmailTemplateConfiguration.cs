using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class EmailTemplateConfiguration : IEntityTypeConfiguration<EmailTemplate>
{
    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
        BaseEntityConfiguration.Configure(builder);

        builder.ToTable(nameof(EmailTemplate));

        builder.HasKey(et => et.Id);
        builder.Property(et => et.Id)
            .ValueGeneratedOnAdd();

        builder.Property(et => et.Name)
            .IsRequired()
            .HasMaxLength(255);
        builder.HasIndex(et => et.Name)
            .IsUnique();

        builder.Property(et => et.Content)
            .IsRequired()
            .HasMaxLength(65535); // i want NVARCHAR(MAX)
    }
}
