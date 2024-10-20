using System.Security.Claims;

namespace TadaTodo.Server.Helpers;

public static class UserHelpers
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var idClaim = user.FindFirst("Id")?.Value;
        if (idClaim is null) return 0;
        return !int.TryParse(idClaim, out var id) ? 0 : id;
    }

    public static string? GetUserName(this ClaimsPrincipal user)
    {
        return user.Identity?.Name;
    }
}