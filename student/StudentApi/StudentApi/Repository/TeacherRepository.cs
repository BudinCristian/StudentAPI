using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.AppDbContext;
using StudentApi.ContractInterface;
using StudentApi.DTOs;
using StudentApi.Entities;
using System.Data;
using System.Security.Cryptography;

namespace StudentApi.Repository
{
    public class TeacherRepository : ITeacherRepository
    {

        private readonly StudentDbcontext _studentDbcontext;

        public TeacherRepository(StudentDbcontext studentDbcontext)
        {
            _studentDbcontext = studentDbcontext;
        }

        public async Task<Teacher> CreateTeacher(TeacherInputDto teacherInput)
        {

            AuthRepository.CreatePasswordHash(teacherInput.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new Teacher
            {
                Username = teacherInput.Username,
                FirstName = teacherInput.FirstName,
                LastName = teacherInput.LastName,
                Email = teacherInput.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "User"
            };

            _studentDbcontext.Teachers.Add(user);
            await _studentDbcontext.SaveChangesAsync();

            return user;
        }

        public async Task<List<Teacher>> GetAllTeacher()
        {
            var allTeachers = await _studentDbcontext.Teachers.ToListAsync();
            return allTeachers;
        }

        public async Task<Teacher> GetTeacherByUsername(string username)
        {
            var existingTeacher = await _studentDbcontext.Teachers.FirstOrDefaultAsync(s => s.Username == username);
            return existingTeacher;
        }

        public async Task<Teacher> GetTeacherById(int id)
        {
            var teacher = await _studentDbcontext.Teachers.FirstOrDefaultAsync(s => s.Id == id);
            return teacher;
        }

        public async Task<bool> IsTeacherExisting(int id)
        {
            var doesTeacherExist = await _studentDbcontext.Teachers.AnyAsync(s => s.Id == id);
            return doesTeacherExist;
        }

        public async Task UpdateTeacher(int id, TeacherInputDto teacherInput)
        {
            AuthRepository.CreatePasswordHash(teacherInput.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var teacher = await _studentDbcontext.Teachers.FirstOrDefaultAsync(s => s.Id == id);
            teacher.Username = teacherInput.Username;
            teacher.FirstName = teacherInput.FirstName;
            teacher.LastName = teacherInput.LastName;
            teacher.Email = teacherInput.Email;
            teacher.PasswordHash = passwordHash;
            teacher.PasswordSalt = passwordSalt;
            teacher.Role = "User";

            await _studentDbcontext.SaveChangesAsync();
        }

        public async Task DeleteTeacher(int id)
        {
            var teacherToDelete = await _studentDbcontext.Teachers.FirstOrDefaultAsync(s => s.Id == id);
            _studentDbcontext.Remove(teacherToDelete);
            await _studentDbcontext.SaveChangesAsync();
        }
    }

}
