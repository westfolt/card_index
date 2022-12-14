using card_index_BLL.Infrastructure;
using card_index_BLL.Models.Data;
using card_index_DAL.Data;
using card_index_Web_API.Extensions;
using card_index_Web_API.Filters;
using card_index_Web_API.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace card_index_Web_API
{
#pragma warning disable CS1591
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
            var connectionString = Configuration.GetConnectionString("CardIndexConnectionString");
            BllDependencyConfigurator.ConfigureServices(services, connectionString);
            services.AddScoped<ValidationFilter>();
            services.AddControllers(options =>
            {
                options.Filters.Add<CardIndexExceptionFilter>();
            });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.ConfigureSwagger();
            services.ConfigureCors();
            services.ConfigureAuthentication(Configuration, 2);
            services.ConfigureValidation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CardIndex API");
                c.RoutePrefix = string.Empty;
            });
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseCors("CardCorsPolicy");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<CardIndexDbContext>();
                if (context != null && !context.Database.IsInMemory())
                {
                    DataSeed.Seed(scope);
                }
            }
        }
    }
}
