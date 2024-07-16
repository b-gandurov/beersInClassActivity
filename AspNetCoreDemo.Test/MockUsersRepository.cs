using System.Collections.Generic;
using System.Linq;

using AspNetCoreDemo.Exceptions;
using AspNetCoreDemo.Models;
using AspNetCoreDemo.Repositories;
using Moq;

namespace AspNetCoreDemo.Tests
{
    public class MockUsersRepository
    {
        public Mock<IUsersRepository> GetMockRepository()
        {
            // Create a mock object for IUsersRepository
            var mockRepository = new Mock<IUsersRepository>();

            // Sample data
            var sampleUsers = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "User1",
                    Password = "password1",
                    FirstName = "First1",
                    LastName = "Last1",
                    Email = "user1@example.com",
                    IsAdmin = false,
                    Beers = new List<Beer>(),
                    Ratings = new List<Rating>()
                },
                new User
                {
                    Id = 2,
                    Username = "User2",
                    Password = "password2",
                    FirstName = "First2",
                    LastName = "Last2",
                    Email = "user2@example.com",
                    IsAdmin = false,
                    Beers = new List<Beer>(),
                    Ratings = new List<Rating>()
                },
                new User
                {
                    Id = 3,
                    Username = "AdminUser",
                    Password = "password3",
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@example.com",
                    IsAdmin = true,
                    Beers = new List<Beer>(),
                    Ratings = new List<Rating>()
                }
            };

            // Setup for GetAll
            mockRepository.Setup(x => x.GetAll())
                .Returns(sampleUsers);

            // Setup for GetById
            mockRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((int id) => sampleUsers.FirstOrDefault(user => user.Id == id));

            // Throw exception when there is no user with ID
            mockRepository
               .Setup(repo => repo.GetById(It.Is<int>(id => sampleUsers.All(u => u.Id != id))))
               .Throws(new EntityNotFoundException("User doesn't exist."));

            // Setup for GetByUsername
            mockRepository.Setup(x => x.GetByUsername(It.IsAny<string>()))
                .Returns((string username) => sampleUsers.FirstOrDefault(user => user.Username == username));

            // Throw exception when there is no user with username
            mockRepository
               .Setup(repo => repo.GetByUsername(It.Is<string>(username => sampleUsers.All(u => u.Username != username))))
               .Throws(new EntityNotFoundException("User doesn't exist."));

            return mockRepository;
        }
    }
}
