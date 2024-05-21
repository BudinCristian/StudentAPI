using StudentApi.DTOs;
using StudentApi.Entities;

namespace StudentApi.ContractInterface
{
    public interface IGroupRepository
    {
        public Task<Group> CreateGroup(Group groupInput);

        public Task<List<Group>> GetAllGroup();

        public Task<Group> GetGroupById(int id);

        public Task UpdateGroup(int id, Group groupInput);

        public Task DeleteGroup(int id);

        public Task<bool> IsGroupExisting(int id);
    }
}