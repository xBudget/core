using Microsoft.Extensions.DependencyInjection;
using xBudget.Identity.Api.Services;

namespace xBudget.Identity.Api.Extensions
{
    public static class ServicesRegisterExtension
    {
        public static void UseCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
