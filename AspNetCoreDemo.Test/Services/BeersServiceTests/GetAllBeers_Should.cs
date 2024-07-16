using AspNetCoreDemo.Repositories;
using AspNetCoreDemo.Services;
using Moq;

namespace AspNetCoreDemo.Tests.Services.BeersServiceTests
{
    [TestClass]
    public class GetAllBeers_Should
    {
        private Mock<IBeersRepository> _mockRepository;
        private BeersService _sut;

        [TestInitialize]
        public void Setup()
        {
            this._mockRepository = new MockBeersRepository().GetMockRepository();
            this._sut = new BeersService(this._mockRepository.Object);
        }

        [TestMethod]
        public void ReturnAllBeers()
        {
            // Arrange
            var expectedBeers = this._mockRepository.Object.GetAll();

            // Act
            var actualBeers = this._sut.GetAll();

            // Assert
            CollectionAssert.AreEqual(expectedBeers, actualBeers);
        }
    }
}
