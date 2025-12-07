using System.Security.Claims;

namespace ShoppingList.List.Web.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub")
            ?? user.FindFirstValue("preferred_username"); // если когда-то захочешь юзать логин
    }

    public static Guid? GetUserIdAsGuid(this ClaimsPrincipal user)
    {
        var raw = user.GetUserId();
        return Guid.TryParse(raw, out var guid) ? guid : null;
    }
}
