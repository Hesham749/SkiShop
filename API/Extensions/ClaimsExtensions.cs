using System.Security.Authentication;
using System.Security.Claims;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ClaimsExtensions
    {
        public static async Task<AppUser> GetUserByEmailAsync(this UserManager<AppUser> UserManager, ClaimsPrincipal user)
        {
            return await UserManager.FindByEmailAsync(user.GetUserEmail())
                ?? throw new AuthenticationException("User not found");
        }

        public static async Task<AppUser> GetUserWithAddressByEmailAsync(this UserManager<AppUser> UserManager, ClaimsPrincipal user)
        {
            return await UserManager.Users.Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Email == user.GetUserEmail())
                ?? throw new AuthenticationException("User not found");
        }

        public static string GetUserEmail(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.Email)
                ?? throw new AuthenticationException("Email claim not found");
    }
}
