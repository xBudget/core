using System.Net.Http;
using System.Threading.Tasks;
using xBudget.Idenitty.Test.Core;
using xBudget.Identity.Api.Models;
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
        }

        [Fact]
        public async Task Register_InvalidEmail()
        {
            var result = await _client.PostAsJsonAsync("/api/auth/register", new UserRegisterViewModel
            {
                Email = "invaidemail"
            });

            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Register_WeakPassword()
        {
            var result = await _client.PostAsJsonAsync("/api/auth/register", new UserRegisterViewModel
            {
                Password = "123456"
            });

            Assert.False(result.IsSuccessStatusCode);
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
        }

        [Fact]
        public async Task Register_Success()
        {
            var result = await _client.PostAsJsonAsync("/api/auth/register", new UserRegisterViewModel
            {
                Email = "email@email.com",
                Password = "Teste@123!",
                PasswordConfirmation = "Teste@123!"
            });

            Assert.True(result.IsSuccessStatusCode);
        }
    }
}
