using AuthService.Service;

namespace AuthServiceTests
{
    public class AuthServiceTest
    {
        [Fact]
        public void ShouldCreateAnUser()
        {
            var authService = new AuthServices();
            var result = authService.CreateUserAsync(new AuthService.DTO.UserDTO
            {
                Email = "email@deTeste.com",
                Password = "senhaDeTeste",
                IsAdmin = false
            });

            Assert.NotNull(result);
        }
        [Fact]
        public void ShouldGetSomeUsers()
        {
            var authService = new AuthServices();
            var result = authService.GetUsersAsync();

            Assert.NotNull(result);
            Assert.Equal(new List<AuthService.DTO.UserDTO>(), result.Result);
        }
    }
}