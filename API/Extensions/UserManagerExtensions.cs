using System.Security.Claims;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser?> FindUserByClaimsPrincipleWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            if (email is null)
                return null;
            
            return await userManager.Users
                .Include(u => u.Address)
                .SingleOrDefaultAsync(u => u.NormalizedEmail == email.ToUpper());
        }

        public static async Task<AppUser?> FindUserByClaimsPrincipleAsync(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            if (email is null)
                return null;
            
            return await userManager.Users
                .Include(u => u.Address)
                .SingleOrDefaultAsync(u => u.NormalizedEmail == email.ToUpper());
        }
    }
}