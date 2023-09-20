using Auth.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimpleTaskBoard.Infrastructure;
using SimpleTaskBoard.Infrastructure.Interfaces;
using SimpleTaskBoard.Infrastructure.Repositories;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var authOptions = builder.Configuration.GetSection("Auth").Get<AuthOptions>();
        var connectionString = builder.Configuration.GetConnectionString("DBConnection");

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
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(j =>
        {
            j.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = authOptions.Issuer,
                ValidAudience = authOptions.Audience,
                IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });
        builder.Services.AddAuthorization();

        var app = builder.Build();

        app.UseRouting();
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapGet("/hello", () => "Content.API");
        app.UseEndpoints(endpoints =>
        { 
            endpoints.MapControllers();
        });

        app.Run();
    }
}