using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Authentication;

public static class JwtExtensions
{
    public static IServiceCollection AddHttpOnlyOrDefaultJwt(
        this IServiceCollection services,
        TokenValidationParameters tokenValidationParameters)
    {
        // from now on [Authroize] attribute will use JWT Bearer authentication
        AuthenticationBuilder authenticationBuilder
            = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

        authenticationBuilder.AddJwtBearer(options =>
        {
            options.TokenValidationParameters = tokenValidationParameters;

            // by default, JWT Bearer authentication will look for the token in the Authorization header
            // here we override this behavior to look for the token in a HttpOnly cookie instead
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = (MessageReceivedContext context) =>
                {
                    context.Token = context.Request.Cookies["AccessToken"];

                    // fall back to Authorization header automatically.
                    if (string.IsNullOrWhiteSpace(context.Token)) context.Token = null;

                    return Task.CompletedTask;
                }
            };
        });
        return services;
    }
}
