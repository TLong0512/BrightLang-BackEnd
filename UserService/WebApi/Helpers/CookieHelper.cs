namespace WebApi.Helpers;

public static class CookieHelper
{
    public static void SetAuthCookies(
        HttpResponse response,
        string accessToken,
        string refreshToken,
        Guid refreshTokenId)
    {
        DateTime refreshTokenLifetime = DateTime.UtcNow.AddDays(7);
        DateTime accessTokenLifetime = DateTime.UtcNow.AddMinutes(10);

        var refreshOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = refreshTokenLifetime,
            Path = "/api/Authentication"
        };

        response.Cookies.Append(Constant.RefreshTokenName, refreshToken, refreshOptions);
        response.Cookies.Append(Constant.RefreshTokenIdName, refreshTokenId.ToString(), refreshOptions);

        response.Cookies.Append(Constant.AccessTokenName, accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = accessTokenLifetime,
            Path = "/"
        });
    }
}
