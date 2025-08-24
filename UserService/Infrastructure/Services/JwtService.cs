using Application.Abstraction;
using Application.Abstraction.Services;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IConfiguration configuration;

    public JwtService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        this.unitOfWork = unitOfWork;
        this.configuration = configuration;
    }

    public async Task<string> GenerateJwtToken(User user)
    {
        // get that user's roles.
        IEnumerable<UserRole> userRoles = await unitOfWork.UserRoles
            .FindAsync(ur => ur.UserId == user.Id);
        IEnumerable<Guid> roleIds = userRoles
            .Select(ur => ur.RoleId);
        IEnumerable<Role> roles = await unitOfWork.Roles
            .FindAsync(r => roleIds.Contains(r.Id));

        // now to the access token (jwt)
        var accessTokenHandler = new JwtSecurityTokenHandler();
        var accessTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                .. roles.Select(r => new Claim(ClaimTypes.Role, r.RoleName)),
            ]),
            Expires = DateTime.UtcNow.AddMinutes(10),
            Issuer = configuration["JwtSettings:Issuer"],
            Audience = configuration["JwtSettings:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)),
                SecurityAlgorithms.HmacSha256
            )
        };
        var accessToken = accessTokenHandler.CreateToken(accessTokenDescriptor);
        var accessTokenString = accessTokenHandler.WriteToken(accessToken);

        return accessTokenString;
    }
}
