namespace StudentApi.Entities
{
    public class LoginUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
