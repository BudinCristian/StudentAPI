using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentApi.AppDbContext;
using StudentApi.ContractInterface;
using StudentApi.DTOs;

namespace StudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly StudentDbcontext _studentDbcontext;
        private readonly IAdminRepository _adminRepository;

        public AdminsController(StudentDbcontext studentDbcontext, IAdminRepository adminRepository)
        {
            _studentDbcontext = studentDbcontext;
            _adminRepository = adminRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AdminInputDto adminInputDto)
        {

            var existingAdmin = await _adminRepository.GetAdminByUsername(adminInputDto.Username);
            if (existingAdmin != null)
            {
                return BadRequest("Username already exists");
            }

            var admin = await _adminRepository.CreateAdmin(adminInputDto);

            return Ok(admin);
        }
        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var allAdmins = await _adminRepository.GetAllAdmin();

            return Ok(allAdmins);
        }

        [HttpGet("{adminId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdminById(int adminId)
        {
            var doesAdminExist = await _adminRepository.IsAdminExisting(adminId);
            if (doesAdminExist == false)
            {
                return NotFound();
            }

            var admin = await _adminRepository.GetAdminById(adminId);

            var newAdminOutputDto = new AdminOutputDto()
            {
                Username = admin.Username,
            };

            return Ok(newAdminOutputDto);
        }

        [HttpPut("{adminId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAdmin(int adminId, [FromBody] AdminInputDto adminInputDto)
        {
            var doesAdminExist = await _adminRepository.IsAdminExisting(adminId);
            if (doesAdminExist == false)
            {
                return NotFound();
            }

            var existingAdmin = await _adminRepository.GetAdminByUsername(adminInputDto.Username);
            if (existingAdmin != null && existingAdmin.Id != adminId)
            {
                return BadRequest("Username already exists");
            }


            await _adminRepository.UpdateAdmin(adminId, adminInputDto);
            return Ok();
        }

        [HttpDelete("{adminId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAdmin(int adminId)
        {
            var doesAdminExist = await _adminRepository.IsAdminExisting(adminId);
            if (doesAdminExist == false)
            {
                return NotFound();
            }

            await _adminRepository.DeleteAdmin(adminId);

            return Ok();
        }
    }
}
