using Microsoft.EntityFrameworkCore;
using StudentApi.AppDbContext;
using StudentApi.ContractInterface;
using StudentApi.DTOs;
using StudentApi.Entities;

namespace StudentApi.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentDbcontext _studentDbcontext;

        public StudentRepository(StudentDbcontext studentDbcontext)
        {
            _studentDbcontext = studentDbcontext;
        }

        public async Task<Student> CreateStudent(Student studentInput)
        {
            await _studentDbcontext.Students.AddAsync(studentInput);
            await _studentDbcontext.SaveChangesAsync();

            return studentInput;
        }
        public async Task<List<Student>> GetAllStudent()
        {
            var allStudents = await _studentDbcontext.Students.ToListAsync();
            return allStudents;
        }

        public async Task<Student> GetStudentById(int id)
        {
            var student = await _studentDbcontext.Students.FirstOrDefaultAsync(s => s.Id == id);
            return student;
        }

        public async Task<bool> IsStudentExisting(int id)
        {
            var doesStudentExist = await _studentDbcontext.Students.AnyAsync(s => s.Id == id);
            return doesStudentExist;
        }

        public async Task UpdateStudent(int id, Student studentInput)
        {
            var student = await _studentDbcontext.Students.FirstOrDefaultAsync(s => s.Id == id);
            student.Email = studentInput.Email;
            student.FirstName = studentInput.FirstName;
            student.LastName = studentInput.LastName;
            student.Age = studentInput.Age;
            student.GroupId = studentInput.GroupId;

            await _studentDbcontext.SaveChangesAsync();
        }

        public async Task DeleteStudent(int id)
        {
            var studentToDelete = await _studentDbcontext.Students.FirstOrDefaultAsync(s => s.Id == id);
            _studentDbcontext.Remove(studentToDelete);
            await _studentDbcontext.SaveChangesAsync();
        }
    }
}