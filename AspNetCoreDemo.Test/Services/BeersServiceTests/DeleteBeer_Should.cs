using System.Collections.Generic;

using AspNetCoreDemo.Exceptions;
using AspNetCoreDemo.Models;
using AspNetCoreDemo.Repositories;
using AspNetCoreDemo.Services;
using Moq;


namespace AspNetCoreDemo.Tests.Services.BeersServiceTests
{
    [TestClass]
    public class DeleteBeer_Should
    {
        private Mock<IBeersRepository> _mockRepository;
        private BeersService _sut;
        private Beer beerToDelete;

        [TestInitialize]
        public void Setup()
        {
            var mockExamples = new MockBeersRepository();
            this._mockRepository = mockExamples.GetMockRepository();
            this._sut = new BeersService(this._mockRepository.Object);
            this.beerToDelete = new Beer
            {
                Id = 2,
                Name = "Beer2",
                Abv = 5.0,
                StyleId = 2,
                Style = new Style { Id = 2, Name = "Lager" },
                UserId = 2, // Created by User2
                User = new User { Id = 2, Username = "User2" },
                Ratings = new List<Rating>
                {
                    new Rating { Id = 2, Value = 4, UserId = 2, User = new User { Id = 2, Username = "User2" } }
                }
            };
        }

        [TestMethod]
        public void ThrowException_When_UserNotCreator()
        {
            // Arrange
            var user = TestHelper.GetTestUser(); // Not the creator of Beer2

            // Act & Assert
            Assert.ThrowsException<UnauthorizedOperationException>(() => this._sut.Delete(this.beerToDelete.Id, user));
        }

        [TestMethod]
        public void DeleteBeer_When_UserNotCreatorButIsAdmin()
        {
            var user = TestHelper.TestUserAdmin();

            // Act
            var success = this._sut.Delete(this.beerToDelete.Id, user);

            // Assert
            Assert.IsTrue(success);
        }
    }
}
