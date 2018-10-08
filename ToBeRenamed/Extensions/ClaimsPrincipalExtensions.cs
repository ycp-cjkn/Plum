using System.Security.Claims;

namespace ToBeRenamed.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(Name);
        }

        public static string GetNameIdentifier(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(NameIdentifier);
        }

        private const string Name = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        private const string NameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    }
}
