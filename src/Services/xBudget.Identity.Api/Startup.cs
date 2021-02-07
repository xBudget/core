using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using xBudget.Identity.Api.Database;
using xBudget.Identity.Api.Extensions;
using xBudget.Lib.Authentication;
using Microsoft.EntityFrameworkCore;

namespace xBudget.Identity.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        { 
            services.AddDbContext<IdentityDatabaseContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityDatabaseContext>()
                .AddDefaultTokenProviders();

            services.UseJwtAuthentication(Configuration);
            services.UseCustomServices();
            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("xBudget.Identity", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "xBudget Identity API."
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/xBudget.Identity/swagger.json", "xBudget.Identity");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                using (var databaseContext = services.GetService<IdentityDatabaseContext>())
                {
                    // skipe migration if the provider is InMemory (provider used on tests)
                    if (databaseContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                    {
                        databaseContext.Database.Migrate();
                    }                    
                }
            }
        }
    }
}
