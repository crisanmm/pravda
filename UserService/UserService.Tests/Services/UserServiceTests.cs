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
        private UserAccountService _userService;
        private AuthenticateModel authenticateModel;

        [Test]
        public void Authenticate_WithEmailAndPasswordExist_ReturnsCorectUser()
        {
            //Arrange
            string expectedEmail = "nassar1234@gamil.com";
            string expectedPassword = "qwe12345";


            //Act
             _userService.Authenticate(expectedEmail, expectedPassword);

            //Assert
            string actualEmail  = authenticateModel.Email;
            Assert.AreEqual(expectedEmail, actualEmail);
        }

    }
}
