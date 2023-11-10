using AuthService.DTO;
using AuthService.Interfaces;
using AuthService.Service;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;

namespace AuthServiceTests
{
    public class AuthServiceTest
    {
        [Fact]
        public void ShouldCreateAnUser()
        {
            var authService = new Mock<IAuthService>();
            var result = authService.Setup(m => m.CreateUserAsync(It.IsAny<UserDTO>()))
                .Callback<UserDTO>(arg => new UserDTO
                {
                    Email = "teste@teste.com",
                    Password = "senhateste",
                    IsAdmin = false
                }).Returns<UserDTO>(x => Task.FromResult(x));

            Assert.NotNull(result);
        }
    }
}