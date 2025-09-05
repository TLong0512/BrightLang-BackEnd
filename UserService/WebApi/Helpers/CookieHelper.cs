namespace WebApi.Helpers;

public static class CookieHelper
{
    private static TimeSpan RefreshTokenLifetime => TimeSpan.FromDays(7);
    private static TimeSpan AccessTokenLifetime => TimeSpan.FromMinutes(10);

    private static CookieOptions GenerateRefreshCookieOptions()
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/api/Authentication"
        };
    }

    private static CookieOptions GenerateAccessCookieOptions()
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/"
        };
    }

    public static void SetAuthCookies(
        HttpResponse response,
        string accessToken,
        string refreshToken,
        Guid refreshTokenId)
    {
        CookieOptions refreshOptions = GenerateRefreshCookieOptions();
        refreshOptions.Expires = DateTime.UtcNow.Add(RefreshTokenLifetime);
        refreshOptions.MaxAge = RefreshTokenLifetime;

        CookieOptions accessOptions = GenerateAccessCookieOptions();
        accessOptions.Expires = DateTime.UtcNow.Add(AccessTokenLifetime);
        accessOptions.MaxAge = AccessTokenLifetime;

        response.Cookies.Append(Constant.RefreshTokenName, refreshToken, refreshOptions);
        response.Cookies.Append(Constant.RefreshTokenIdName, refreshTokenId.ToString(), refreshOptions);
        response.Cookies.Append(Constant.AccessTokenName, accessToken, accessOptions);
    }

    public static void RemoveAuthCookies(HttpResponse response)
    {
        CookieOptions refreshOptions = GenerateRefreshCookieOptions();
        refreshOptions.Expires = DateTime.UtcNow.AddDays(-1);
        //refreshOptions.MaxAge = TimeSpan.FromMinutes(-1);

        CookieOptions accessOptions = GenerateAccessCookieOptions();
        accessOptions.Expires = DateTime.UtcNow.AddDays(-1);
        //accessOptions.MaxAge = TimeSpan.FromMinutes(-1);

        response.Cookies.Delete(Constant.RefreshTokenIdName, refreshOptions);
        response.Cookies.Delete(Constant.RefreshTokenName, refreshOptions);
        response.Cookies.Delete(Constant.AccessTokenName, accessOptions);
    }

}
