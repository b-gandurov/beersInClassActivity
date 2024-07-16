using System.Collections.Generic;

using AspNetCoreDemo.Exceptions;
using AspNetCoreDemo.Models;
using AspNetCoreDemo.Repositories;
using AspNetCoreDemo.Services;
using Moq;


namespace AspNetCoreDemo.Tests.Services.BeersServiceTests
{
    [TestClass]
    public class UpdateBeer_Should
    {
        private Mock<IBeersRepository> _mockRepository;
        private BeersService _sut;
        private Beer updatedBeer;

        [TestInitialize]
        public void Setup()
        {
            var mockExamples = new MockBeersRepository();
            this._mockRepository = mockExamples.GetMockRepository();
            this._sut = new BeersService(this._mockRepository.Object);
            this.updatedBeer = new Beer
            {
                Id = 1,
                Name = "UpdatedBeer",
                Abv = 5.5,
                StyleId = 1,
                Style = new Style { Id = 1, Name = "IPA" },
                UserId = 1,
                User = new User { Id = 1, Username = "User1" },
                Ratings = new List<Rating>()
            };
        }

        [TestMethod]
        public void ReturnUpdatedBeer_When_ParamsAreValid()
        {
            // Act
            var actualBeer = this._sut.Update(this.updatedBeer.Id, this.updatedBeer, this.updatedBeer.User);

            // Assert
            Assert.AreEqual(this.updatedBeer, actualBeer);
        }

        [TestMethod]
        public void ThrowException_When_UserNotAuthorized()
        {
            // Arrange
            var unauthorizedUser = new User { Id = 2, Username = "User2" }; // Not the owner

            // Act & Assert
            Assert.ThrowsException<UnauthorizedOperationException>(() => this._sut.Update(this.updatedBeer.Id, this.updatedBeer, unauthorizedUser));
        }

        [TestMethod]
        public void UpdateBeer_When_UserIsAdmin()
        {
            // Arrange
            var adminUser = TestHelper.TestUserAdmin(); // Admin user

            // Act
            var actualBeer = this._sut.Update(this.updatedBeer.Id, this.updatedBeer, adminUser);

            // Assert
            Assert.AreEqual(this.updatedBeer, actualBeer);
        }
    }
}
