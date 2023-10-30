using Api.Models.Entities;
using Api.Repositories;
using Moq;
using Api.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Microsoft.EntityFrameworkCore;
using Api.Contexts;

namespace UnitTests
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task GetByUsernameAsync_WhenUsernameExists_ShouldReturnUserEntity()
        {
            //Arrange
            string username = "user1";
            Mock<ILogger<IUserRepository>> iLoggerMock = new Mock<ILogger<IUserRepository>>();
            Mock<DbSet<UserEntity>> dbSetMock = GetFakeUserList().BuildMock().BuildMockDbSet();
            dbSetMock.Setup(x => x.FirstOrDefaultAsync(CancellationToken.None)).ReturnsAsync(GetFakeUserList().FirstOrDefault());
            /*
             mock.Setup(x => x.FindAsync(1)).ReturnsAsync(
        TestDataHelper.GetFakeEmployeeList().Find(e => e.Id == 1));
             */

            Mock<DataContext> dataContextMock = new Mock<DataContext>();
            dataContextMock.Setup(x => x.Users).Returns(dbSetMock.Object);

            UserRepository userRepository = new UserRepository(dataContextMock.Object, iLoggerMock.Object);

            //Act
            UserEntity user = await userRepository.GetByUsernameAsync(username);

            //Assert
            Assert.NotNull(user);
            Assert.Equal(username, user.Username);
        }

        private static List<UserEntity> GetFakeUserList()
        {
            var users = new List<UserEntity>()
            {
                new UserEntity() { Username = "user1", Password = "", Score = 0 },
                new UserEntity() { Username = "user2", Password = "", Score = 0 },
                new UserEntity() { Username = "user3", Password = "", Score = 0 },
                new UserEntity() { Username = "user4", Password = "", Score = 0 },
                new UserEntity() { Username = "user5", Password = "", Score = 0 }
            };

            return users;
        }

    }
}

