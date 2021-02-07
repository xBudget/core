using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using xBudget.Account.Api.Database;
using xBudget.Account.Api.Models.Api;
using xBudget.Lib.Test;
using xBudget.Lib.Test.Extensions;
using xBudget.Lib.Test.Model;
using Xunit;

namespace xBudget.Account.Test.ApiTest
{
    public class AccountControllerTest : IClassFixture<ApplicationFactory<Api.Startup, AccountDatabaseContext>>
    {
        private readonly HttpClient _client;

        public AccountControllerTest(ApplicationFactory<Api.Startup, AccountDatabaseContext> factory)
        {
            _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }


        [Fact]
        public async Task CreateAccount_EmptyData()
        {
            var result = await _client.PostAsJsonAsync("/api/account/create", new CreateAccountApiModel
            {

            });

            Assert.False(result.IsSuccessStatusCode);

            var errorData = await result.Content.ReadAsObjectAsync<ErrorResult>();

            Assert.NotNull(errorData);
            Assert.Contains("Name", errorData.Errors.Select(x => x.Key));
            Assert.Contains("Description", errorData.Errors.Select(x => x.Key));
        }
    }
}
