using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using xBudget.Identity.Api.Extensions;
using xBudget.Identity.Api.Models;
using xBudget.Identity.Api.Models.Core;
using xBudget.Lib.Authentication;

namespace xBudget.Identity.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AuthSettings _authSettings;

        public AuthService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
                              IOptions<AuthSettings> options)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _authSettings = options.Value;
        }

        public async Task<ServiceResult<string>> Login(UserLoginViewModel viewModel)
        {
            var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, false, true);
            if (result.Succeeded)
            {
                return new ServiceResult<string>
                {
                    Result = await GenerateJwt(viewModel.Email)
                };
            }

            return new ServiceResult<string>
            {
                Errors = new Dictionary<string, IList<string>>
                {
                    { "", new List<string> { "Invalid Email or Password" }}
                }
            };
        }

        public async Task<ServiceResult<string>> Register(UserRegisterViewModel viewModel)
        {
            var user = new IdentityUser
            {
                UserName = viewModel.Email,
                Email = viewModel.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, viewModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return new ServiceResult<string>
                {
                    Result = await GenerateJwt(viewModel.Email)
                };
            }

            var errorResult = new ServiceResult<string>();
            foreach (var error in result.Errors)
            {
                errorResult.Errors.Add(error.Code, new List<string> { error.Description });
            }

            return errorResult;
        }

        private async Task<string> GenerateJwt(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            claims.Add(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            foreach (var role in roles)
            {
                claims.Add(new System.Security.Claims.Claim("role", role));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _authSettings.Issuer,
                Audience = _authSettings.Audience,
                Subject = new System.Security.Claims.ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_authSettings.Expiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }
    }
}
