using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using StackExchange.Redis;
using System.Security.Claims;
using System.Text;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.MiddleWare;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.RepostriesInterfaces;
using Talabat.Core.ServiceInterface;
using Talabat.Core.Services.Interfaces.ServiceInterface;
using Talabat.Repositry;
using Talabat.Repositry.Data;
using Talabat.Repositry.Identity;
using Talabat.Repositry.Identity.DataSeed;
using Talabat.Repositry.Repositres;
using Talabat.Services;

namespace Talabat.APIs
{
    public class Program
    {
        //Entry Point
        public static async Task Main(string[] args)
        {
            var WebApplicationbuilder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            WebApplicationbuilder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            WebApplicationbuilder.Services.AddEndpointsApiExplorer();
            WebApplicationbuilder.Services.AddSwaggerGen();

            WebApplicationbuilder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(WebApplicationbuilder.Configuration.GetConnectionString(name:"DefaultConnection"));
            });


            WebApplicationbuilder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(WebApplicationbuilder.Configuration.GetConnectionString(name: "IdentityConnection"));
            });
            WebApplicationbuilder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidIssuer = WebApplicationbuilder.Configuration["JWT:ValidIssuer"],
         ValidateAudience = true,
         ValidAudience = WebApplicationbuilder.Configuration["JWT:Audience"],
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(WebApplicationbuilder.Configuration["JWT:Key"])),
         RoleClaimType = ClaimTypes.Role, // Maps role claims
         NameClaimType = ClaimTypes.Email // Maps email as name claim
     };
 });


            //WebApplicationbuilder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //     .AddJwtBearer(options => {
            //         options.TokenValidationParameters = new TokenValidationParameters()
            //         {
            //             ValidateIssuer = true,
            //             ValidIssuer = WebApplicationbuilder.Configuration["JWT:ValidIssues"],
            //             ValidateAudience = true,
            //             ValidAudience = WebApplicationbuilder.Configuration["JWT:Audience"],
            //             ValidateLifetime = true,
            //             ValidateIssuerSigningKey = true,
            //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(WebApplicationbuilder.Configuration["JWT:Key"]))
            //         };

            //     });


            WebApplicationbuilder.Services.AddSingleton<IConnectionMultiplexer>((serviceBrovider) =>
            {
               var connection= WebApplicationbuilder.Configuration.GetConnectionString(name: "Redis");
                return ConnectionMultiplexer.Connect(connection);
            });
            WebApplicationbuilder.Services.AddScoped(typeof(IBasketRepositry), typeof(BasketRepositry));
            WebApplicationbuilder.Services.AddScoped<ITokennService, TokenService>();
            WebApplicationbuilder.Services.AddScoped<IOrderService, OrderService>();
            WebApplicationbuilder.Services.AddScoped<IUnitOfWork, UnitOWork>();
            //WebApplicationbuilder.Services.AddScoped<IUnitOfWork, UnitOWork>();


            WebApplicationbuilder.Services.AddApplicationService();
            WebApplicationbuilder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {

            }).AddEntityFrameworkStores<AppIdentityDbContext>();


            var app = WebApplicationbuilder.Build();
            var scope=app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _context= services.GetRequiredService<StoreDbContext>();

            var loggerFactory=services.GetRequiredService<ILoggerFactory>();
            try
            {
               await _context.Database.MigrateAsync();
               await StoreDbContextSeed.SeedAsync(_context);

                var _userManager=services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUsersAsync(_userManager);
            }

            catch (Exception ex) 
            {
               var logger= loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, message: "An Error has been Occured During the Migrations");
                Console.WriteLine(ex.Message);
            }

            // Configure the HTTP request pipeline

            app.UseMiddleware<ExceptionMiddleWare>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStatusCodePagesWithReExecute(pathFormat: "/errors/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
