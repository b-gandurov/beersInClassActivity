using System.Collections.Generic;

using AspNetCoreDemo.Exceptions;
using AspNetCoreDemo.Models;
using AspNetCoreDemo.Repositories;
using AspNetCoreDemo.Services;
using Moq;

namespace AspNetCoreDemo.Tests.Services.BeersServiceTests
{
    [TestClass]
    public class CreateBeer_Should
    {
        private Mock<IBeersRepository> _mockRepository;
        private BeersService _sut;
        private Beer createdBeer;
        private User createBy;

        [TestInitialize]
        public void Setup()
        {
            var mockExamples = new MockBeersRepository();
            this._mockRepository = mockExamples.GetMockRepository();
            this._sut = new BeersService(this._mockRepository.Object);
            this.createdBeer = new Beer
            {
                Id = 3,
                Name = "NewBeer",
                Abv = 5.5,
                StyleId = 3,
                Style = new Style { Id = 3, Name = "Stout" },
                UserId = 3,
                User = new User { Id = 3, Username = "User3" },
                Ratings = new List<Rating>()
            };

            this.createBy = TestHelper.TestUserAdmin();
        }

        [TestMethod]
        public void ReturnCorrectBeer_When_ParamsAreValid()
        {  
            // Act
            var actualBeer = this._sut.Create(this.createdBeer, this.createBy);

            //Assert
            Assert.AreEqual(this.createdBeer, actualBeer);
        }

        [TestMethod]
        public void Throw_When_BeernameAlreadyExists()
        {
            // Arrange
            Beer duplicateBeer = TestHelper.GetTestBeer();
            var testUser = TestHelper.GetTestUser();
            // Act & Assert
            Assert.ThrowsException<DuplicateEntityException>(() => this._sut.Create(duplicateBeer, testUser));
        }

        [TestMethod]
        public void CallBeerRepository()
        {
            // Act
            var actualBeer = this._sut.Create(this.createdBeer, this.createBy);

            // Assert
            this._mockRepository.Verify(x => x.Create(this.createdBeer), Times.Once);
        }
    }
}