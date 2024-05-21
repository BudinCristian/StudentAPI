namespace StudentApi.Entities
{
    public class Admin : LoginUser
    {
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
    }
}
