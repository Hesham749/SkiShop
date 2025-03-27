using System.Security.Cryptography.Xml;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using API.Middleware;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers().AddJsonOptions(op =>
            {
                op.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<StoreContext>(op =>
            {
                op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddCors();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                var connString = builder.Configuration.GetConnectionString("Redis")
                ?? throw new Exception("Cannot get redis connection string");

                var configuration = ConfigurationOptions.Parse(connString, true);

                return ConnectionMultiplexer.Connect(configuration);
            });
            builder.Services.AddIdentityApiEndpoints<AppUser>(op =>
            {
                op.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<StoreContext>();

            builder.Services.AddSingleton<ICartService, CartService>();

            builder.Services.AddScoped<IPaymentService, PaymentService>();

            //builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionMiddleware>();


            //app.UseHttpsRedirection();


            //app.UseAuthorization();

            app.UseCors(op =>
            {
                op.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
                .WithOrigins("https://localhost:4200", "http://localhost:4200");
            });

            app.MapControllers();
            app.MapGroup("api").MapIdentityApi<AppUser>();


            try
            {
                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetService<StoreContext>();
                if (context is not null)
                {
                    await context.Database.MigrateAsync();
                    await StoreContextSeed.SeedAsync(context);
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
