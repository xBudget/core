using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace xBudget.Lib.Test
{
    public class ApplicationFactory<TStartup, TDatabaseContext> : WebApplicationFactory<TStartup> where TStartup : class where TDatabaseContext : DbContext
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var applicationDbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TDatabaseContext>));
                services.Remove(applicationDbContextDescriptor);

                services.AddDbContext<TDatabaseContext>(options => options.UseInMemoryDatabase("InMemoryDbForTesting"));
            });
        }
    }
}
