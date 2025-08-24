using Application.Abstraction.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Abstraction;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    IUserRoleRepository UserRoles { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IOAuthLoginRepository OAuthLogins { get; }
    IVerificationCodeRepository VerificationCodes { get; }
    IEmailTemplateRepository EmailTemplates { get; }

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(
        System.Data.IsolationLevel isolationLevel, 
        CancellationToken cancellationToken);

    Task<int> SaveChangesAsync();
}
