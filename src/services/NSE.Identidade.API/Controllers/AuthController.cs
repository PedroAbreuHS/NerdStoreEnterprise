using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSE.Identidade.API.Models;

namespace NSE.Identidade.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(UserRegisterViewModel userRegisterViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new IdentityUser
            {
                UserName = userRegisterViewModel.Email,
                Email = userRegisterViewModel.Email,
                EmailConfirmed = true
            };

            var result = _userManager.CreateAsync(user, userRegisterViewModel.Senha);

            if (result.IsCompletedSuccessfully)
            {
                await _signInManager.SignInAsync(user, false);
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("autenticar")]
        public async Task<ActionResult> Login(UserLoginViewModel userLoginViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = _signInManager.PasswordSignInAsync(userLoginViewModel.Email, userLoginViewModel.Senha, false, true);

            if (result.IsCompletedSuccessfully)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
