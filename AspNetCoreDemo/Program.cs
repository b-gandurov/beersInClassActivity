using AspNetCoreDemo.Data;
using AspNetCoreDemo.Helpers;
using AspNetCoreDemo.Repositories;
using AspNetCoreDemo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace AspNetCoreDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews() // Uncomment the lines below if you get the exception "JsonSerializationException: Self referencing loop detected for property..." 
                // .AddNewtonsoftJson(options =>
                // {
                //     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                // })
                ;

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Beers API", Version = "v1" });
            });

            // EF 
            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                // The connection string can be found in the appsettings.json file. 
                // It's a good practice to keep the connection string in a separate file,
                //  because it's easier to change the connection string without recompiling the entire application.
                // Also, the connection string is a sensitive information and should not be exposed in the code.
                string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
                
                // The following helps with debugging the trobled relationship between EF and SQL ¯\_(-_-)_/¯ 
                options.EnableSensitiveDataLogging();
            });

            // Repositories
            builder.Services.AddScoped<IBeersRepository, BeersRepository>();
            builder.Services.AddScoped<IStylesRepository, StylesRepository>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();

            // Services
            builder.Services.AddScoped<IBeersService, BeersService>();
            builder.Services.AddScoped<IStylesService, StylesService>();
            builder.Services.AddScoped<IUsersService, UsersService>();

            // Helpers
            builder.Services.AddScoped<ModelMapper>();
            builder.Services.AddScoped<AuthManager>();

            var app = builder.Build();

            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "AspNetCoreDemo API V1");
                options.RoutePrefix = "api/swagger";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            app.Run();
        }
    }
}
