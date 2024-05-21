using StudentApi.DTOs;
using StudentApi.Entities;

namespace StudentApi.ContractInterface
{
    public interface IAuthRepository
    {
        Task<AuthResponseDto> Login(LoginDto request);
        Task<AuthResponseDto> RefreshToken();
    }
}
