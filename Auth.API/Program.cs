using Auth.Common;
using Microsoft.EntityFrameworkCore;
using SimpleTaskBoard.Infrastructure;
using SimpleTaskBoard.Infrastructure.Interfaces;
using SimpleTaskBoard.Infrastructure.Repositories;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var authSection = builder.Configuration.GetSection("Auth");
        var connectionString = builder.Configuration.GetConnectionString("DBConnection");

        builder.Services.Configure<AuthOptions>(authSection);
        builder.Services.AddControllers();
        builder.Services.AddCors(option =>
        {
            option.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        builder.Services.AddDbContext<SimpleTaskBoardDbContext>(options 
            => options.UseNpgsql(connectionString));
        builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

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