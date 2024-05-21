using StudentApi.DTOs;
using StudentApi.Entities;

namespace StudentApi.ContractInterface
{
    public interface ITeacherRepository
    {
        public Task<Teacher> CreateTeacher(TeacherInputDto teacherInput);

        public Task<List<Teacher>> GetAllTeacher();

        public Task<Teacher> GetTeacherByUsername(string username);

        public Task<Teacher> GetTeacherById(int id);

        public Task UpdateTeacher(int id, TeacherInputDto teacherInput);

        public Task DeleteTeacher(int id);

        public Task<bool> IsTeacherExisting(int id);
    }
}
