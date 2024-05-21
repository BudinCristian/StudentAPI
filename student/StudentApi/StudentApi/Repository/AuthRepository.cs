using StudentApi.Entities;
using StudentApi.ContractInterface;
using Microsoft.IdentityModel.Tokens;
using StudentApi.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using StudentApi.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace StudentApi.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly StudentDbcontext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthRepository(StudentDbcontext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResponseDto> Login(LoginDto request)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(u => u.Username == request.Username);
            var admin = await _context.Admins.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (teacher != null)
            {
                if (!VerifyPasswordHash(request.Password, teacher.PasswordHash, teacher.PasswordSalt))
                {
                    return new AuthResponseDto { Message = "Wrong Password." };
                }

                string token = CreateToken(teacher);
                var refreshToken = CreateRefreshToken();
                await SetRefreshToken(refreshToken, teacher);

                return new AuthResponseDto
                {
                    Success = true,
                    Token = token,
                    RefreshToken = refreshToken.Token,
                    TokenExpires = refreshToken.Expires
                };
            }
            else
            {
                if(admin != null)
                {
                    if (!VerifyPasswordHash(request.Password, admin.PasswordHash, admin.PasswordSalt))
                    {
                        return new AuthResponseDto { Message = "Wrong Password." };
                    }

                    string token = CreateToken(admin);
                    var refreshToken = CreateRefreshToken();
                    await SetRefreshToken(refreshToken, admin);

                    return new AuthResponseDto
                    {
                        Success = true,
                        Token = token,
                        RefreshToken = refreshToken.Token,
                        TokenExpires = refreshToken.Expires
                    };
                }

                return new AuthResponseDto { Message = "User not found." };
            }
        }

        public async Task<AuthResponseDto> RefreshToken()
        {
            var refreshToken = _httpContextAccessor?.HttpContext?.Request.Cookies["refreshToken"];
            var user = await _context.Teachers.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user == null)
            {
                return new AuthResponseDto { Message = "Invalid Refresh Token" };
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return new AuthResponseDto { Message = "Token expired." };
            }

            string token = CreateToken(user);
            var newRefreshToken = CreateRefreshToken();
            await SetRefreshToken(newRefreshToken, user);

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                RefreshToken = newRefreshToken.Token,
                TokenExpires = newRefreshToken.Expires
            };
        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private string CreateToken(LoginUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private RefreshToken CreateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private async Task SetRefreshToken(RefreshToken refreshToken, LoginUser user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires,
            };
            _httpContextAccessor?.HttpContext?.Response
                .Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

            user.RefreshToken = refreshToken.Token;
            user.TokenCreated = refreshToken.Created;
            user.TokenExpires = refreshToken.Expires;

            await _context.SaveChangesAsync();
        }
    }
}