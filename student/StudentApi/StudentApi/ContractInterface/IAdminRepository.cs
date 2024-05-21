using StudentApi.DTOs;
using StudentApi.Entities;

namespace StudentApi.ContractInterface
{
    public interface IAdminRepository
    {
        public Task<Admin> CreateAdmin(AdminInputDto adminInput);

        public Task<List<Admin>> GetAllAdmin();

        public Task<Admin> GetAdminByUsername(string username);

        public Task<Admin> GetAdminById(int id);

        public Task UpdateAdmin(int id, AdminInputDto adminInput);

        public Task DeleteAdmin(int id);

        public Task<bool> IsAdminExisting(int id);
    }
}
