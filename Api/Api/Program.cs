using Api.BackgroundServices;
using Api.Clients;
using Api.Clients.Interfaces;
using Api.Contexts;
using Api.Models.DDragonClasses;
using Api.Repositories;
using Api.Repositories.Interfaces;
using Api.Services;
using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

            //Clients
            builder.Services.AddHttpClient<IDDragonCdnClient, DDragonCdnClient>();

            //Background services
            builder.Services.AddHostedService<DDragonCdnBackgroundService>();

            //Services
            builder.Services.AddSingleton<IDDragonCdnService, DDragonCdnService>();
            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddSingleton<IJwtService, JwtService>();

            //Repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var app = builder.Build();
            
            app.UseCors(x => x.WithOrigins("https://localhost:5000", "http://localhost:3000").WithHeaders("content-type", "authorization"));
            
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