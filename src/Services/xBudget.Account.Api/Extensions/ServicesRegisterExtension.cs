using Microsoft.Extensions.DependencyInjection;
using xBudget.Account.Api.Database.Repository;

namespace xBudget.Account.Api.Extensions
{
    public static class ServicesRegisterExtension
    {
        public static void UseCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
        }
    }
}
