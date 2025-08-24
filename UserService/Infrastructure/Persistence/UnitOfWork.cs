using Application.Abstraction;
using Application.Abstraction.Repositories;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence;


public class UnitOfWork : IUnitOfWork, IDisposable
{
    protected readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    private IUserRepository? _users;
    public IUserRepository Users => _users ??= new UserRepository(_context);



    private IRoleRepository? _roles;
    public IRoleRepository Roles => _roles ??= new RoleRepository(_context);



    private IUserRoleRepository? _userRoles;
    public IUserRoleRepository UserRoles => _userRoles ??= new UserRoleRepository(_context);



    private IRefreshTokenRepository? _refreshTokens;
    public IRefreshTokenRepository RefreshTokens => _refreshTokens ??= new RefreshTokenRepository(_context);



    private IOAuthLoginRepository? _oAuthLogins;
    public IOAuthLoginRepository OAuthLogins => _oAuthLogins ??= new OAuthLoginRepository(_context);



    private IVerificationCodeRepository? _verificationCodes;
    public IVerificationCodeRepository VerificationCodes => _verificationCodes ??= new VerificationCodeRepository(_context);


    private IEmailTemplateRepository? _emailTemplates;
    public IEmailTemplateRepository EmailTemplates => _emailTemplates ??= new EmailTemplateRepository(_context);


    public async Task<IDbContextTransaction> BeginTransactionAsync(
        System.Data.IsolationLevel isolationLevel, 
        CancellationToken cancellationToken)
    {
        return await _context.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Database.BeginTransactionAsync(cancellationToken);
    }


    public async Task<int> SaveChangesAsync()
    {
        int result = await _context.SaveChangesAsync();
        return result;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}