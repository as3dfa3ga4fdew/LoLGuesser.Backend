using Api.Contexts;
using Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Infrastructure
{
    public class DataContextTestBase : IDisposable
    {
        protected readonly DataContext _dataContext;

        public DataContextTestBase()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            _dataContext = new DataContext(options);

            _dataContext.Database.EnsureCreated();


            //Seed
            var users = new[]
            {
                new UserEntity() { Username = "user1", Password = "", Score = 0 },
                new UserEntity() { Username = "user2", Password = "", Score = 0 },
                new UserEntity() { Username = "user3", Password = "", Score = 0 },
                new UserEntity() { Username = "user4", Password = "", Score = 0 },
                new UserEntity() { Username = "user5", Password = "", Score = 0 }
            };

            _dataContext.Users.AddRange(users);
            _dataContext.SaveChanges();
        }
        public void Dispose() 
        {
            _dataContext.Database.EnsureDeleted();
            _dataContext.Dispose();
        }
    }
}
