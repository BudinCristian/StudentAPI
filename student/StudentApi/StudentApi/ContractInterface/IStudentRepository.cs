using StudentApi.DTOs;
using StudentApi.Entities;

namespace StudentApi.ContractInterface
{
    public interface IStudentRepository
    {
        public Task<Student> CreateStudent(Student studentInput);

        public Task<List<Student>> GetAllStudent();

        public Task<Student> GetStudentById(int id);

        public Task UpdateStudent(int id, Student studentInput);

        public Task DeleteStudent(int id);

        public Task<bool> IsStudentExisting(int id);
    }
}