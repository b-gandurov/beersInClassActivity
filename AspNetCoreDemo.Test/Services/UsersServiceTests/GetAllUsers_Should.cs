using AspNetCoreDemo.Repositories;
using AspNetCoreDemo.Services;
using Moq;

namespace AspNetCoreDemo.Tests.Services.UsersServiceTests
{
    [TestClass]
    public class GetAllUsers_Should
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
        public void ReturnAllUsers()
        {
            // Arrange
            var expectedUsers = this._mockRepository.Object.GetAll();

            // Act
            var actualUsers = this._sut.GetAll();

            // Assert
            CollectionAssert.AreEqual(expectedUsers, actualUsers);
        }
    }
}
