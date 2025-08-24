using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class Seed
{
    public static async Task ApplyAsync(
        AppDbContext context,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        // Ensure database is created and migrations are applied
        await context.Database.MigrateAsync(cancellationToken);

        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        bool hasUserAndRoleSeeded = await context.Users.AnyAsync(cancellationToken);
        if (!hasUserAndRoleSeeded)
        {
            Role role1 = new Role { RoleName = "User" };
            Role role2 = new Role { RoleName = "Admin" };
            await context.Roles.AddRangeAsync(role1, role2);
            await context.SaveChangesAsync(cancellationToken);

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            string adminFullName = configuration["AdminAccount:Username"] ?? "Admin";
            string adminEmail = configuration["AdminAccount:Email"] ?? "admin@example.com";
            string adminPassword = configuration["AdminAccount:Password"] ?? "Abc123!@#";
            string adminPasswordHashed = BCrypt.Net.BCrypt.HashPassword(adminPassword);
            // for demonstrate only. in production, we must provide seed admin account
            // from .env or else.

            // Seed Admin account
            User user = new User
            {
                FullName = adminFullName,
                Email = adminEmail,
                Password = adminPasswordHashed,
            };
            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            // then add UserRole for that account
            UserRole userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role2.Id
            };
            await context.UserRoles.AddAsync(userRole, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        // Update EmailTemplate table
        string templatesPath = Path.Combine(AppContext.BaseDirectory, "EmailTemplates");
        string[] fileDirectories = Directory.GetFiles(templatesPath, "*.html");
        // remember to add this into this project's .csproj:
        // <ItemGroup>
        //   <Content Include="EmailTemplates\**\*">
        //     <CopyToOutputDirectory>Always</CopyToOutputDirectory>
        //   </Content>
        // </ItemGroup>

        foreach (var fileDirectory in fileDirectories)
        {
            string name = Path.GetFileName(fileDirectory);
            string content = await File.ReadAllTextAsync(fileDirectory, cancellationToken);

            EmailTemplate? existingEmailTemplate = await context.EmailTemplates
                .FirstOrDefaultAsync(t => t.Name == name, cancellationToken);

            if (existingEmailTemplate == null)
            {
                context.EmailTemplates.Add(new EmailTemplate
                {
                    Name = name,
                    Content = content
                }); // add
            }
            else
            {
                existingEmailTemplate.Content = content; // update 
            }
            // remain templates untouched.
        }
        await context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }
}

