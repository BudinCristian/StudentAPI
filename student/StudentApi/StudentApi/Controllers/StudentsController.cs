using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.AppDbContext;
using StudentApi.ContractInterface;
using StudentApi.DTOs;
using StudentApi.Entities;

namespace StudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentDbcontext _studentDbcontext;
        private readonly IStudentRepository _studentRepository;
        private readonly IGroupRepository _groupRepository;

        public StudentsController(StudentDbcontext studentDbcontext, IStudentRepository studentRepository, IGroupRepository groupRepository)
        {
            _studentDbcontext = studentDbcontext;
            _studentRepository = studentRepository;
            _groupRepository = groupRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentInputDto studentInputDto)
        {
            if (!await _groupRepository.IsGroupExisting(studentInputDto.GroupId))
            {
                return NotFound();
            }

            var newStudent = new Student()
            {
                Age = studentInputDto.Age,
                Email = studentInputDto.Email,
                FirstName = studentInputDto.FirstName,
                LastName = studentInputDto.LastName,
                GroupId = studentInputDto.GroupId
            };

            var student = await _studentRepository.CreateStudent(newStudent);

            return Ok(student);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var allStudents = await _studentRepository.GetAllStudent();

            return Ok(allStudents);
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudentById(int studentId)
        {
            var doesStudentExist = await _studentRepository.IsStudentExisting(studentId);
            if (doesStudentExist == false)
            {
                return NotFound();
            }

            var student = await _studentRepository.GetStudentById(studentId);

            var newStudentOutputDto = new StudentOutputDto()
            {
                LastName = student.LastName,
                FirstName = student.FirstName,
                Email = student.Email
            };

            return Ok(newStudentOutputDto);
        }

        [HttpPut("{studentId}")]
        public async Task<IActionResult> UpdateStudent(int studentId, [FromBody] StudentInputDto studentInputDto)
        {
            var doesStudentExist = await _studentRepository.IsStudentExisting(studentId);
            if (doesStudentExist == false)
            {
                return NotFound();
            }

            if (!await _groupRepository.IsGroupExisting(studentInputDto.GroupId))
            {
                return NotFound();
            }

            var newStudent = new Student()
            {
                FirstName = studentInputDto.FirstName,
                LastName = studentInputDto.LastName,
                Age = studentInputDto.Age,
                Email = studentInputDto.Email,
                GroupId = studentInputDto.GroupId
            };

            await _studentRepository.UpdateStudent(studentId, newStudent);
            return Ok();
        }

        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var doesStudentExist = await _studentRepository.IsStudentExisting(studentId);
            if (doesStudentExist == false)
            {
                return NotFound();
            }

            await _studentRepository.DeleteStudent(studentId);

            return Ok();
        }
    }
}