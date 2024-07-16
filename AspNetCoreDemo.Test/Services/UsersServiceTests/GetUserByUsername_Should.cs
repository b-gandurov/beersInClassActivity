using AspNetCoreDemo.Exceptions;
using AspNetCoreDemo.Repositories;
using AspNetCoreDemo.Services;
using Moq;

namespace AspNetCoreDemo.Tests.Services.UsersServiceTests
{
    [TestClass]
    public class GetUserByUsername_Should
    {
        private Mock<IUsersRepository> _mockRepository;
        private UsersService _sut;

        [TestInitialize]
        public void Setup()
        {
            var mockExamples = new MockUsersRepository();
            this._mockRepository = mockExamples.GetMockRepository();
            this._sut = new UsersService(this._mockRepository.Object);
        }

        [TestMethod]
        public void ReturnCorrectUser_When_ValidParameters()
        {
            // Arrange
            var expectedUser = this._mockRepository.Object.GetByUsername("User1");

            // Act
            var actualUser = this._sut.GetByUsername("User1");

            // Assert
            Assert.AreEqual(expectedUser, actualUser);
        }

        [TestMethod]
        public void ThrowException_When_UserNotFound()
        {
            // Act & Assert
            Assert.ThrowsException<EntityNotFoundException>(() => this._sut.GetByUsername("NonExistentUser"));
        }
    }

}
