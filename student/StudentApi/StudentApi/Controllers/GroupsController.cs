using Microsoft.AspNetCore.Mvc;
using StudentApi.AppDbContext;
using StudentApi.ContractInterface;
using StudentApi.DTOs;
using StudentApi.Entities;

namespace StudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly StudentDbcontext _studentDbcontext;
        private readonly IGroupRepository _groupRepository;

        public GroupsController(StudentDbcontext studentDbcontext, IGroupRepository groupRepository)
        {
            _studentDbcontext = studentDbcontext;
            _groupRepository = groupRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GroupInputDto groupInputDto)
        {
            var newGroup = new Group()
            {
                Name = groupInputDto.Name,
                Description = groupInputDto.Description
            };

            var group = await _groupRepository.CreateGroup(newGroup);

            return Ok(group);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var allGroups = await _groupRepository.GetAllGroup();

            return Ok(allGroups);
        }

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetGroupById(int groupId)
        {
            var doesGroupExist = await _groupRepository.IsGroupExisting(groupId);
            if (doesGroupExist == false)
            {
                return NotFound();
            }

            var group = await _groupRepository.GetGroupById(groupId);

            var newGroupOutputDto = new GroupOutputDto()
            {
                Name = group.Name,
                Description = group.Description
            };

            return Ok(newGroupOutputDto);
        }

        [HttpPut("{groupId}")]
        public async Task<IActionResult> UpdateGroup(int groupId, [FromBody] GroupInputDto groupInputDto)
        {
            var doesGroupExist = await _groupRepository.IsGroupExisting(groupId);
            if (doesGroupExist == false)
            {
                return NotFound();
            }

            var newGroup = new Group()
            {
                Name = groupInputDto.Name,
                Description = groupInputDto.Description
            };

            await _groupRepository.UpdateGroup(groupId, newGroup);
            return Ok();
        }

        [HttpDelete("{groupId}")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var doesGroupExist = await _groupRepository.IsGroupExisting(groupId);
            if (doesGroupExist == false)
            {
                return NotFound();
            }

            await _groupRepository.DeleteGroup(groupId);

            return Ok();
        }
    }
}
