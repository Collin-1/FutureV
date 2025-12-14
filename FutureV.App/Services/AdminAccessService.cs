using Microsoft.AspNetCore.Http;

namespace FutureV.Services;

public class AdminAccessService
{
    private const string AdminCookieName = "FutureV.AdminAccess";
    private const string AdminPassphrase = "admin";

    public bool HasAccess(HttpContext context)
    {
        return context.Request.Cookies.TryGetValue(AdminCookieName, out var value) && value == "granted";
    }

    public bool ValidatePassphrase(string passphrase)
    {
        return string.Equals(passphrase?.Trim(), AdminPassphrase, StringComparison.Ordinal);
    }

    public void GrantAccess(HttpContext context)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = context.Request.IsHttps,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(4)
        };

        context.Response.Cookies.Append(AdminCookieName, "granted", options);
    }

    public void RevokeAccess(HttpContext context)
    {
        context.Response.Cookies.Delete(AdminCookieName);
    }
}
