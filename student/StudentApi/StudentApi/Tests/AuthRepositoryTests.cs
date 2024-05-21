using Xunit;
using StudentApi.Repository;
using System.Security.Cryptography;
using System.Linq;

namespace StudentApi.Tests
{
    public class AuthRepositoryTests
    {
        [Fact]
        public void Test_CreatePasswordHash()
        {
            string password = "testPassword";
            byte[] passwordHash;
            byte[] passwordSalt;

            AuthRepository.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            Assert.NotNull(passwordHash);
            Assert.NotNull(passwordSalt);
        }

        [Fact]
        public void Test_VerifyPasswordHash()
        {
            string password = "testPassword";
            byte[] passwordHash;
            byte[] passwordSalt;
            AuthRepository.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            bool isVerified = AuthRepository.VerifyPasswordHash(password, passwordHash, passwordSalt);

            Assert.True(isVerified);
        }
    }
}