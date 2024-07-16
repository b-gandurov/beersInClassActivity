using AspNetCoreDemo.Models;
using AspNetCoreDemo.Repositories;
using AspNetCoreDemo.Services;
using Moq;

namespace AspNetCoreDemo.Tests.Services.BeersServiceTests
{
    [TestClass]
    public class FilterBeers_Should
    {
        private Mock<IBeersRepository> _mockRepository;
        private BeersService _sut;
        private BeerQueryParameters filterParameters;

        [TestInitialize]
        public void Setup()
        {
            var mockExamples = new MockBeersRepository();
            this._mockRepository = mockExamples.GetMockRepository();
            this._sut = new BeersService(this._mockRepository.Object);
            this.filterParameters = new BeerQueryParameters { Name = "Beer", Style = "IPA", MinAbv = 4.0, MaxAbv = 6.0 };
        }

        [TestMethod]
        public void ReturnFilteredBeers_When_ParamsAreValid()
        {
            // Arrange
            var expectedBeers = this._mockRepository.Object.FilterBy(this.filterParameters);

            // Act
            var actualBeers = this._sut.FilterBy(this.filterParameters);

            // Assert
            CollectionAssert.AreEqual(expectedBeers, actualBeers);
        }

        [TestMethod]
        public void ReturnEmptyList_When_NoBeersMatchFilter()
        {
            // Arrange
            var noMatchParams = new BeerQueryParameters { Name = "NonExistentBeer" };

            // Act
            var actualBeers = this._sut.FilterBy(noMatchParams);

            // Assert
            Assert.AreEqual(0, actualBeers.Count);
        }
    }
}
