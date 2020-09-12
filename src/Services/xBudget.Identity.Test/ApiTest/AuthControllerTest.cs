using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using xBudget.Idenitty.Test.Core;
using xBudget.Identity.Api.Models;
using xBudget.Identity.Test.Core.Model;
using xBudget.Lib.Test.Extensions;
using Xunit;

namespace xBudget.Idenitty.Test.ApiTest
{
    public class AuthControllerTest : IClassFixture<ApplicationFactory<Identity.Api.Startup>>
    {
        private readonly HttpClient _client;
        private readonly ApplicationFactory<Identity.Api.Startup> _factory;

        public AuthControllerTest(ApplicationFactory<Identity.Api.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Register_EmptyData()
        {
            var result = await _client.PostAsJsonAsync("/api/auth/register", new UserRegisterViewModel
            {

            });

            Assert.False(result.IsSuccessStatusCode);

            var errorData = await result.Content.ReadAsObjectAsync<ErrorResult>();
            
            Assert.NotNull(errorData);
            Assert.Contains("Email", errorData.Errors.Select(x => x.Key));
            Assert.Contains("Password", errorData.Errors.Select(x => x.Key));
        }

        [Fact]
        public async Task Register_InvalidEmail()
        {
            var result = await _client.PostAsJsonAsync("/api/auth/register", new UserRegisterViewModel
            {
                Email = "invaidemail"
            });

            Assert.False(result.IsSuccessStatusCode);

            var errorData = await result.Content.ReadAsObjectAsync<ErrorResult>();

            Assert.NotNull(errorData);
            Assert.Contains("Email", errorData.Errors.Select(x => x.Key));
        }

        [Fact]
        public async Task Register_WeakPassword()
        {
            var result = await _client.PostAsJsonAsync("/api/auth/register", new UserRegisterViewModel
            {
                Password = "123456",
                PasswordConfirmation = "123456",
                Email = "email@email.com"
            });

            Assert.False(result.IsSuccessStatusCode);

            var errorData = await result.Content.ReadAsObjectAsync<ErrorResult>();

            Assert.NotNull(errorData);
            Assert.Contains("PasswordRequiresNonAlphanumeric", errorData.Errors.Select(x => x.Key));
        }

        [Fact]
        public async Task Register_WrongPasswordConfirmation()
        {
            var result = await _client.PostAsJsonAsync("/api/auth/register", new UserRegisterViewModel
            {
                Password = "123456",
                PasswordConfirmation = "654321"
            });

            Assert.False(result.IsSuccessStatusCode);

            var errorData = await result.Content.ReadAsObjectAsync<ErrorResult>();

            Assert.NotNull(errorData);
            Assert.Contains("PasswordConfirmation", errorData.Errors.Select(x => x.Key));
        }

        [Fact]
        public async Task Register_Success()
        {
            var result = await RegisterUser(Guid.NewGuid().ToString());
            Assert.True(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Login_InvalidData()
        {
            var guid = Guid.NewGuid().ToString();
            await RegisterUser(guid);

            var result = await _client.PostAsJsonAsync("/api/auth/login", new UserLoginViewModel
            {
                Email = "wrong@email.com",
                Password = "Teste@123!",
            });

            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Login_Success()
        {
            var guid = Guid.NewGuid().ToString();
            await RegisterUser(guid);

            var result = await _client.PostAsJsonAsync("/api/auth/login", new UserLoginViewModel
            {
                Email = $"{ guid }@email.com",
                Password = "Teste@123!",
            });

            Assert.True(result.IsSuccessStatusCode);

            var responseData = await result.Content.ReadAsObjectAsync<TokenResponse>();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("MySecret123qweEWQ#@!");

            try
            {

                tokenHandler.ValidateToken(responseData.Token, new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                Assert.False(true);
            }
            catch (Exception ex)
            {
                Assert.False(false);
            }
        }

        private async Task<HttpResponseMessage> RegisterUser(string emailPrefix)
        {
            var result = await _client.PostAsJsonAsync("/api/auth/register", new UserRegisterViewModel
            {
                Email = $"{ emailPrefix }@email.com",
                Password = "Teste@123!",
                PasswordConfirmation = "Teste@123!"
            });

            return result;
        }

        public class TokenResponse
        {
            public string Token { get; set; }
        }
    }
}
