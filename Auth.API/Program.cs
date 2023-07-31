using Auth.API;
using Auth.API.Models;
using Auth.API.Repositories;
using Auth.Common;
using Microsoft.EntityFrameworkCore;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var authSection = builder.Configuration.GetSection("Auth");
        var connectionString = builder.Configuration.GetConnectionString("DBConnection");

        builder.Services.AddControllers();
        builder.Services.Configure<AuthOptions>(authSection);
        builder.Services.AddCors(option =>
        {
            option.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        builder.Services.AddDbContext<AuthContext>(options 
            => options.UseNpgsql(connectionString));
        builder.Services.AddTransient<IRepository<User>, UserRepository>();

        var app = builder.Build();

        app.UseRouting();
        app.UseCors();
        app.MapGet("/hello", () => "Auth.API");
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.Run();
    }
}