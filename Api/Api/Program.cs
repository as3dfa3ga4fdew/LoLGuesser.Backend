using Api.BackgroundServices;
using Api.Clients;
using Api.Clients.Interfaces;
using Api.Contexts;
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

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DataContext>(x =>
            x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

            //Clients
            builder.Services.AddHttpClient<IDDragonCdnClient, DDragonCdnClient>();

            //Background services
            builder.Services.AddHostedService<DDragonCdnBackgroundService>();

            //Services
            builder.Services.AddSingleton<IDDragonCdnService, DDragonCdnService>();

            //Repositories


            var app = builder.Build();

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