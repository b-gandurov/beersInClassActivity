using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AspNetCoreDemo.Exceptions;
using AspNetCoreDemo.Helpers;
using AspNetCoreDemo.Models;
using AspNetCoreDemo.Services;
using Moq;

namespace AspNetCoreDemo.Tests.AuthManagerTests
{
    [TestClass]
    public class TryGetUser_Should
    {
        private Mock<IUsersService> _usersServiceMock;
        private AuthManager _authManager;

        [TestInitialize]
        public void SetUp()
        {
            this._usersServiceMock = new Mock<IUsersService>();
            this._authManager = new AuthManager(this._usersServiceMock.Object);
        }

        [TestMethod]
        public void ReturnCorrectUser_When_ValidCredentials()
        {
            var expectedUser = new User { Username = "testUser", Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("password")) };
            this._usersServiceMock.Setup(s => s.GetByUsername("testUser")).Returns(expectedUser);

            var result = this._authManager.TryGetUser("testUser:password");

            Assert.AreEqual(expectedUser, result);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedOperationException))]
        public void ThrowUnauthorizedOperationException_When_InvalidPassword()
        {
            var user = new User { Username = "testUser", Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("wrongPassword")) };
            this._usersServiceMock.Setup(s => s.GetByUsername("testUser")).Returns(user);

            this._authManager.TryGetUser("testUser:password");
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedOperationException))]
        public void ThrowUnauthorizedOperationException_When_UserNotFound()
        {
            this._usersServiceMock.Setup(s => s.GetByUsername(It.IsAny<string>())).Throws(new EntityNotFoundException());

            this._authManager.TryGetUser("nonExistentUser:password");
        }
    }
}
