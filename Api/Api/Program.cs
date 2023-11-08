using Api.BackgroundServices;
using Api.Clients;
using Api.Clients.Interfaces;
using Api.Contexts;
using Api.Middlewares;
using Api.Models.DDragonClasses;
using Api.Repositories;
using Api.Repositories.Interfaces;
using Api.Services;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DataContext>(x =>
            x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

            builder.Services.AddLogging(x =>
            x.AddConsole());

            builder.Services
              .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = builder.Configuration.GetSection("Jwt").GetValue<string>("Issuer"),
                      ValidAudience = builder.Configuration.GetSection("Jwt").GetValue<string>("Issuer"),
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt").GetValue<string>("Key")!))
                  };
              });

            //Middleware
            builder.Services.AddScoped<ExceptionMiddleware>();

            //Clients
            builder.Services.AddHttpClient<IDDragonCdnClient, DDragonCdnClient>();

            //Background services
            builder.Services.AddHostedService<DDragonCdnBackgroundService>();

            //Services
            builder.Services.AddSingleton<IDDragonCdnService, DDragonCdnService>();
            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IAddressService, AddressService>();
            builder.Services.AddScoped<IUserService, UserService>();

            //Repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAddressRepository, AddressRepository>();

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseCors(x => x.WithOrigins("https://localhost:5000", "http://localhost:3000").WithHeaders("content-type", "authorization"));
            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}