using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentApi.AppDbContext;
using StudentApi.ContractInterface;
using StudentApi.DTOs;
using StudentApi.Entities;

namespace TeacherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly StudentDbcontext _studentDbcontext;
        private readonly ITeacherRepository _teacherRepository;

        public TeachersController(StudentDbcontext studentDbcontext, ITeacherRepository teacherRepository)
        {
            _studentDbcontext = studentDbcontext;
            _teacherRepository = teacherRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeacherInputDto teacherInputDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTeacher = await _teacherRepository.GetTeacherByUsername(teacherInputDto.Username);
            if (existingTeacher != null)
            {
                return BadRequest("Username already exists");
            }

            var teacher = await _teacherRepository.CreateTeacher(teacherInputDto);

            return Ok(teacher);
        }
        [HttpGet, Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetAllTeachers()
        {
            var allTeachers = await _teacherRepository.GetAllTeacher();
        
            var newTeacherOutputDto = allTeachers.Select(teacher => new TeacherOutputDto
            {
                Id = teacher.Id,
                Username = teacher.Username,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Email = teacher.Email
            }).ToArray();
        
            return Ok(newTeacherOutputDto);
        }
        [HttpGet("{teacherId}"), Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetTeacherById(int teacherId)
        {
            var doesTeacherExist = await _teacherRepository.IsTeacherExisting(teacherId);
            if (doesTeacherExist == false)
            {
                return NotFound();
            }

            var teacher = await _teacherRepository.GetTeacherById(teacherId);

            var newTeacherOutputDto = new TeacherOutputDto()
            {
                Id = teacher.Id,
                Username = teacher.Username,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Email = teacher.Email
            };

            return Ok(newTeacherOutputDto);
        }

        [HttpPut("{teacherId}"), Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdateTeacher(int teacherId, [FromBody] TeacherInputDto teacherInputDto)
        {
            var doesTeacherExist = await _teacherRepository.IsTeacherExisting(teacherId);
            if (doesTeacherExist == false)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTeacher = await _teacherRepository.GetTeacherByUsername(teacherInputDto.Username);
            if (existingTeacher != null && existingTeacher.Id != teacherId)
            {
                return BadRequest("Username already exists");
            }


            await _teacherRepository.UpdateTeacher(teacherId, teacherInputDto);
            return Ok();
        }

        [HttpDelete("{teacherId}"), Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> DeleteTeacher(int teacherId)
        {
            var doesTeacherExist = await _teacherRepository.IsTeacherExisting(teacherId);
            if (doesTeacherExist == false)
            {
                return NotFound();
            }

            await _teacherRepository.DeleteTeacher(teacherId);

            return Ok();
        }
    }
}
