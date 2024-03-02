using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RentJunction_API.Business;
using RentJunction_API.Business.Interface;
using RentJunction_API.CustomFilters;
using RentJunction_API.Data;
using RentJunction_API.DataAccess;
using RentJunction_API.DataAccess.Interface;
using RentJunction_API.Middlewares;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace RentJunction_API
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }  
        private async Task CreateUserRole(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var roleName in new[] { "Admin", "Customer", "Owner" })
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = roleName });
                }
            }
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
          
            services.AddControllers();
          
            //services.AddMvc(options => options.Filters.Add<CustomActionFilter>());
            services.AddSwaggerGen();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "RentJunction",
                    Description = "Renting made easy",
                });
            });

            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("RentJunctionDBConnection")));

            services.AddScoped<IAuthBusiness, AuthBusiness>();          
            services.AddScoped<IProductBusiness, ProductBusiness>();
            services.AddScoped<IUserBusiness, UserBusiness>();
            services.AddScoped<IUserData, UserData>();
            services.AddScoped<IProductsData, ProductsData>();
            services.AddScoped<IRentalData, RentalData>();
            services.AddScoped<CustomActionFilter>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder => builder.WithOrigins("http://localhost:4200") 
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials());
            });
            services.AddHttpContextAccessor();

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider,ILoggerProvider logger)
        {
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseCors("AllowOrigin");
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Showing API V1");
            });

            app.UseRouting();
            
           
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
               
                endpoints.MapControllers();
            });

            CreateUserRole(serviceProvider).Wait();
        }
    }
}