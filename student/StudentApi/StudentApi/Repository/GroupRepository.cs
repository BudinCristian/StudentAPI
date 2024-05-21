using Microsoft.EntityFrameworkCore;
using StudentApi.AppDbContext;
using StudentApi.ContractInterface;
using StudentApi.DTOs;
using StudentApi.Entities;

namespace StudentApi.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly StudentDbcontext _studentDbcontext;

        public GroupRepository(StudentDbcontext studentDbcontext)
        {
            _studentDbcontext = studentDbcontext;
        }

        public async Task<Group> CreateGroup(Group groupInput)
        {
            await _studentDbcontext.Groups.AddAsync(groupInput);
            await _studentDbcontext.SaveChangesAsync();

            return groupInput;
        }

        public async Task<List<Group>> GetAllGroup()
        {
            var allGroups = await _studentDbcontext.Groups.ToListAsync();
            return allGroups;
        }

        public async Task<Group> GetGroupById(int id)
        {
            var group = await _studentDbcontext.Groups.FirstOrDefaultAsync(g => g.Id == id);
            return group;
        }

        public async Task<bool> IsGroupExisting(int id)
        {
            var doesGroupExist = await _studentDbcontext.Groups.AnyAsync(g => g.Id == id);
            return doesGroupExist;
        }

        public async Task UpdateGroup(int id, Group groupInput)
        {
            var group = await _studentDbcontext.Groups.FirstOrDefaultAsync(g => g.Id == id);
            group.Name = groupInput.Name;
            group.Description = groupInput.Description;

            await _studentDbcontext.SaveChangesAsync();
        }

        public async Task DeleteGroup(int id)
        {
            var groupToDelete = await _studentDbcontext.Groups.FirstOrDefaultAsync(g => g.Id == id);
            
            _studentDbcontext.Remove(groupToDelete);
            await _studentDbcontext.SaveChangesAsync();
        }
    }
}
