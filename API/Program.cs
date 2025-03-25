using System.Threading.Tasks;
using API.Middleware;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<StoreContext>(op =>
            {
                op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddCors();

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            app.UseMiddleware<ExceptionMiddleware>();
            //app.UseHttpsRedirection();

            //app.UseAuthorization();

            app.UseCors(op =>
            {
                op.AllowAnyHeader().AllowAnyMethod()
                .WithOrigins("https://localhost:4200", "http://localhost:4200");
            });

            app.MapControllers();

            try
            {
                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetService<StoreContext>();
                if (context is not null)
                {
                    await context.Database.MigrateAsync();
                    await StoreContextSeed.SeedProductsAsync(context);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            app.Run();
        }
    }
}
