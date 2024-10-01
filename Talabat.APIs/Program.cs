using Microsoft.EntityFrameworkCore;
using Talabat.Repositry.Data;

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

            var app = WebApplicationbuilder.Build();
            var scope=app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _context= services.GetRequiredService<StoreDbContext>();

            var loggerFactory=services.GetRequiredService<ILoggerFactory>();
            try
            {
               await _context.Database.MigrateAsync();
               await StoreDbContextSeed.SeedAsync(_context);
            }

            catch (Exception ex) 
            {
               var logger= loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, message: "An Error has been Occured During the Migrations");
                Console.WriteLine(ex.Message);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
