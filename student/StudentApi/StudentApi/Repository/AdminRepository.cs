using Microsoft.EntityFrameworkCore;
using StudentApi.AppDbContext;
using StudentApi.ContractInterface;
using StudentApi.DTOs;
using StudentApi.Entities;
using System.Security.Cryptography;

namespace StudentApi.Repository
{
    public class AdminRepository : IAdminRepository
    {

        private readonly StudentDbcontext _studentDbcontext;

        public AdminRepository(StudentDbcontext studentDbcontext)
        {
            _studentDbcontext = studentDbcontext;
        }

        public async Task<Admin> CreateAdmin(AdminInputDto teacherInput)
        {

            AuthRepository.CreatePasswordHash(teacherInput.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new Admin
            {
                Username = teacherInput.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "Admin"
            };

            _studentDbcontext.Admins.Add(user);
            await _studentDbcontext.SaveChangesAsync();

            return user;
        }

        public async Task<List<Admin>> GetAllAdmin()
        {
            var allAdmins = await _studentDbcontext.Admins.ToListAsync();
            return allAdmins;
        }

        public async Task<Admin> GetAdminByUsername(string username)
        {
            var existingAdmin = await _studentDbcontext.Admins.FirstOrDefaultAsync(s => s.Username == username);
            return existingAdmin;
        }

        public async Task<Admin> GetAdminById(int id)
        {
            var teacher = await _studentDbcontext.Admins.FirstOrDefaultAsync(s => s.Id == id);
            return teacher;
        }

        public async Task<bool> IsAdminExisting(int id)
        {
            var doesAdminExist = await _studentDbcontext.Admins.AnyAsync(s => s.Id == id);
            return doesAdminExist;
        }

        public async Task UpdateAdmin(int id, AdminInputDto teacherInput)
        {
            AuthRepository.CreatePasswordHash(teacherInput.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var teacher = await _studentDbcontext.Admins.FirstOrDefaultAsync(s => s.Id == id);
            teacher.Username = teacherInput.Username;
            teacher.PasswordHash = passwordHash;
            teacher.PasswordSalt = passwordSalt;
            teacher.Role = "Admin";

            await _studentDbcontext.SaveChangesAsync();
        }

        public async Task DeleteAdmin(int id)
        {
            var teacherToDelete = await _studentDbcontext.Admins.FirstOrDefaultAsync(s => s.Id == id);
            _studentDbcontext.Remove(teacherToDelete);
            await _studentDbcontext.SaveChangesAsync();
        }
    }
}
