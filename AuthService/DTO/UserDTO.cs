namespace AuthService.DTO
{
    public class UserDTO
    {
        public string UserID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}
