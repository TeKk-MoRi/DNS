using DNS.Application.Common;
using DNS.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DNS.Presentation.Controllers
{
    public class GroupsController : BaseController
    {
        private readonly IKeycloakService _keycloakService;

        public GroupsController(IKeycloakService keycloakService)
        {
            _keycloakService = keycloakService;
        }


        [HttpGet]
        public async Task<ActionResult<Result<List<KeycloakGroupDto>>>> GetAllGroups()
        {
            Result<List<KeycloakGroupDto>> result = await _keycloakService.GetGroupsAsync();

            if (result.IsFailure)
                return BadRequest(Result.Failure(result.Message));

            return Ok(Result<List<KeycloakGroupDto>>.Success(result.Data));
        }

        [HttpPost("{userId}/{groupId}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Result>> AssignToGroup(string userId, string groupId)
        {
            Result<bool> result = await _keycloakService.AssignToGroupAsync(userId, groupId);

            if (result.IsFailure)
                return BadRequest(Result.Failure(result.Message));

            return Ok(Result.Success("User assigned to group successfully"));
        }

        [HttpDelete("{userId}/{groupId}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Result>> RemoveFromGroup(string userId, string groupId)
        {
            Result<bool> result = await _keycloakService.RemoveFromGroupAsync(userId, groupId);

            if (result.IsFailure)
                return BadRequest(Result.Failure(result.Message));

            return Ok(Result.Success("User removed from group successfully"));
        }

    }
}
