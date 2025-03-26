using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(SignInManager<AppUser> signInManager) : BaseApiController
    {
        private readonly SignInManager<AppUser> _signInManager = signInManager;

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var Created = await _signInManager.UserManager.CreateAsync(user, registerDto.Password);
            if (!Created.Succeeded)
            {
                foreach (var error in Created.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }

            return Ok("Account Created");
        }


        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return NoContent();
        }


        [HttpGet("user-info")]
        public async Task<ActionResult> Greet()
        {
            if (!User.Identity?.IsAuthenticated ?? true) return NoContent();

            var user = await _signInManager.UserManager.GetUserWithAddressByEmailAsync(User);

            return Ok(new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                Address = user.Address?.ToDto(),
            });
        }

        [HttpGet]
        public ActionResult GetAuthState()
        {
            return Ok(new { IsAuthenticated = User.Identity?.IsAuthenticated ?? false });
        }

        [Authorize]
        [HttpPost("address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto addressDto)
        {
            var user = await _signInManager.UserManager.GetUserWithAddressByEmailAsync(User);

            if (user.Address is null)
                user.Address = addressDto.ToEntity();
            else
                user.Address.UpdateFromDto(addressDto);

            var result = await _signInManager.UserManager.UpdateAsync(user);
            if (result.Succeeded)
                return addressDto;

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);


            return ValidationProblem();

        }
    }
}
