using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using UserService.Helpers;
using UserService.Models.Users;
using UserService.Services;

namespace UserService.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserAccountService _userService;
        private AuthenticateModel authenticateModel;

        [Test]
        public void Authenticate_WithEmailAndPasswordExist_ReturnsCorectUser()
        {
            //Arrange
            string expectedEmail = "nassarm65@gamil.com";
            string expectedPassword = "20000209Ma";
            Mock<DataContext> mockDataContext = new Mock<DataContext>();
            UserAccountService userAccountService = new UserAccountService(mockDataContext.Object);


            //Act
            _userService.Authenticate(expectedEmail, expectedPassword);

            //Assert
            string actualEmail = authenticateModel.Email;
            Assert.AreEqual(expectedEmail, actualEmail);
        }

    }
}
