using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using xBudget.Identity.Api.Database;

namespace xBudget.Idenitty.Test.Core
{
    public class ApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var applicationDbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DatabaseContext>));
                services.Remove(applicationDbContextDescriptor);

                services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase("InMemoryDbForTesting"));
            });
        }
    }
}
