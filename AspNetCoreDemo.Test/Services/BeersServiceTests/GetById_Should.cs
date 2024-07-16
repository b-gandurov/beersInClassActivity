using AspNetCoreDemo.Exceptions;
using AspNetCoreDemo.Models;
using AspNetCoreDemo.Repositories;
using AspNetCoreDemo.Services;
using Moq;


namespace AspNetCoreDemo.Tests.Services.BeersServiceTests
{
    [TestClass]
    public class GetById_Should
    {
        private Mock<IBeersRepository> _mockRepository;
        private BeersService _sut;

        [TestInitialize]
        public void Setup()
        {
            var mockExamples = new MockBeersRepository();
            this._mockRepository = mockExamples.GetMockRepository();
            this._sut = new BeersService(this._mockRepository.Object);
        }

        [TestMethod]
        public void ReturnCorrectBeer_When_ValidParameters()
        { 
            //Arrange
            Beer expectedBeer = TestHelper.GetTestBeer();

            //Act
            var actualBeer = this._sut.GetById(expectedBeer.Id);

            //Assert
            Assert.AreEqual(expectedBeer, actualBeer);
        }


        [TestMethod]
        public void ThrowException_When_BeerNotFound()
        {
            // Act & Assert
            Assert.ThrowsException<EntityNotFoundException>(() => this._sut.GetById(3));
        }

        //[TestMethod]
        //public void ReturnCorrectBeer_When_ValidParameters()
        //{
        //    //Arrange
        //    Beer expectedBeer = TestHelper.GetTestBeer();

        //    var repositoryMock = new Mock<IBeersRepository>();
        //    repositoryMock
        //        .Setup(r => r.GetById(1))
        //        .Returns(expectedBeer);

        //    BeersService sut = new BeersService(repositoryMock.Object);


        //    //Act
        //    Beer actualBeer = sut.GetById(expectedBeer.Id);

        //    //Assert
        //    Assert.AreEqual(expectedBeer, actualBeer);
        //}

        //[TestMethod]
        //public void ThrowException_When_BeerNotFound()
        //{
        //    // Arrange
        //    var repositoryMock = new Mock<IBeersRepository>();

        //    repositoryMock
        //        .Setup(repo => repo.GetById(It.IsAny<int>()))
        //        .Throws(new EntityNotFoundException("Beer doesn't exist."));

        //    // Act
        //    var sut = new BeersService(repositoryMock.Object);

        //    Assert.ThrowsException<EntityNotFoundException>(() => sut.GetById(1));
        //}
    }
}
