using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreDemo.Controllers.API;
using AspNetCoreDemo.Exceptions;
using AspNetCoreDemo.Helpers;
using AspNetCoreDemo.Models;
using AspNetCoreDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AspNetCoreDemo.Tests.Controllers.BeersApiControllerTests
{
    [TestClass]
    public class Create_Should
    {
        private Mock<IBeersService> _beersServiceMock;
        private Mock<ModelMapper> _modelMapperMock;
        private Mock<AuthManager> _authManagerMock;
        private BeersApiController _controller;
        private Mock<IUsersService> _usersServiceMock;

        [TestInitialize]
        public void SetUp()
        {
            this._beersServiceMock = new Mock<IBeersService>();
            this._modelMapperMock = new Mock<ModelMapper>();

            this._usersServiceMock = new Mock<IUsersService>();
            this._authManagerMock = new Mock<AuthManager>(this._usersServiceMock.Object);

            this._controller = new BeersApiController(this._beersServiceMock.Object, this._modelMapperMock.Object, this._authManagerMock.Object);
        }

        [TestMethod]
        public void ReturnCreatedBeer_When_CredentialsAreValid()
        {
            // Arrange
            string credentials = "User1:password1";
            BeerRequestDto beerRequestDto = TestHelper.GetBeerRequestDto();
            Beer beerEntity = TestHelper.GetTestBeer();
            BeerResponseDto beerResponseDto = TestHelper.GetBeerResponseDto();
            User user = TestHelper.GetTestUser();

            this._usersServiceMock.Setup(x => x.GetByUsername("User1")).Returns(user);
            this._authManagerMock.Setup(x => x.TryGetUser(credentials)).Returns(user);
            this._modelMapperMock.Setup(x => x.Map(beerRequestDto)).Returns(beerEntity);
            this._beersServiceMock.Setup(x => x.Create(beerEntity, user)).Returns(beerEntity);
            this._modelMapperMock.Setup(x => x.Map(beerEntity)).Returns(beerResponseDto);

            // Act
            var result = this._controller.Create(credentials, beerRequestDto);

            // Assert
            var createdResult = result as ObjectResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.AreEqual(beerResponseDto, createdResult.Value);
        }

        //[TestMethod]
        //public void ReturnUnauthorized_When_CredentialsAreInvalid()
        //{
        //    // Arrange
        //    string credentials = "invalid:credentials";
        //    BeerRequestDto beerDto = new BeerRequestDto();

        //    _authManagerMock.Setup(x => x.TryGetUser(credentials)).Throws(new UnauthorizedOperationException());

        //    // Act
        //    var result = _controller.Create(credentials, beerDto);

        //    // Assert
        //    Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        //}

        //[TestMethod]
        //public void ReturnConflict_When_BeerAlreadyExists()
        //{
        //    // Arrange
        //    string credentials = "valid:credentials";
        //    BeerRequestDto beerDto = new BeerRequestDto();
        //    Beer beer = new Beer();
        //    User user = new User();

        //    _authManagerMock.Setup(x => x.TryGetUser(credentials)).Returns(user);
        //    _modelMapperMock.Setup(x => x.Map(beerDto)).Returns(beer);
        //    _beersServiceMock.Setup(x => x.Create(beer, user)).Throws(new DuplicateEntityException());

        //    // Act
        //    var result = _controller.Create(credentials, beerDto);

        //    // Assert
        //    Assert.IsInstanceOfType(result, typeof(ConflictObjectResult));
        //}
    }
}
