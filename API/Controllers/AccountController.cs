using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    [Route("api/[controller]")]
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

            var AddedToRole = await _signInManager.UserManager.AddToRoleAsync(user, "Customer");
            if (!AddedToRole.Succeeded)
            {
                foreach (var error in AddedToRole.Errors)
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

            return Ok();
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
                IsAdmin = User.IsInRole("Admin"),
                Roles = await _signInManager.UserManager.GetRolesAsync(user),
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
