using Auth.Common;
using Microsoft.AspNetCore.Authentication.OAuth;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var authSection = builder.Configuration.GetSection("Auth");

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
        var app = builder.Build();

        app.UseRouting();
        app.UseCors();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.Run();
    }
}