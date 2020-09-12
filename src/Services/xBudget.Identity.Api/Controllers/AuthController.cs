using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using xBudget.Identity.Api.Models;
using xBudget.Identity.Api.Services;

namespace xBudget.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var result = await _authService.Register(viewModel);
                if (result.Success)
                {
                    return Ok(new { token = result.Result });
                }

                return BadRequest(new { errors = result.Errors });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _authService.Login(viewModel);
            if (result.Success)
            {
                return Ok(new { token = result.Result });
            }

            return BadRequest(new { errors = result.Errors });
        }
    }
}
