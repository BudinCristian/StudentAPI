using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentApi.ContractInterface;
using StudentApi.DTOs;
using StudentApi.Entities;

namespace StudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authService;

        public AuthController(IAuthRepository authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<ActionResult<Teacher>> Login(LoginDto request)
        {
            var response = await _authService.Login(request);
            if (response.Success)
                return Ok(response);

            return BadRequest(response.Message);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var response = await _authService.RefreshToken();
            if (response.Success)
                return Ok(response);

            return BadRequest(response.Message);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult<string> Aloha()
        {
            return Ok("Aloha! You're authorized!");
        }
    }
}