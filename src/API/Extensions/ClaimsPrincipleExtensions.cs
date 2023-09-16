using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions 
    {
        public static string RetrieveEmailFromPrinciple(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.Email);
        }
    }
}
