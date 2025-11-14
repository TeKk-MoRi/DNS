using DNS.Application.Common;
using DNS.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DNS.Presentation.Controllers
{
    public class RolesController : BaseController
    {
        private readonly IKeycloakService _keycloakService;

        public RolesController(IKeycloakService keycloakService)
        {
            _keycloakService = keycloakService;
        }


        [HttpGet]
        public async Task<ActionResult<Result<List<KeycloakRoleDto>>>> GetAllRoles()
        {
            Result<List<KeycloakRoleDto>> result = await _keycloakService.GetRealmRolesAsync();

            if (result.IsFailure)
                return BadRequest(Result.Failure(result.Message));

            return Ok(Result<List<KeycloakRoleDto>>.Success(result.Data));
        }

        [HttpPost("{userId}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Result>> AssignRoles(string userId, [FromBody] AssignRolesRequest request)
        {
            Result<bool> result = await _keycloakService.AssignRolesAsync(userId, request.Roles);

            if (result.IsFailure)
                return BadRequest(Result.Failure(result.Message));

            return Ok(Result.Success("Roles assigned successfully"));
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Result>> RemoveRoles(string userId, [FromBody] RemoveRolesRequest request)
        {
            Result<bool> result = await _keycloakService.RemoveRolesAsync(userId, request.Roles);

            if (result.IsFailure)
                return BadRequest(Result.Failure(result.Message));

            return Ok(Result.Success("Roles removed successfully"));
        }
    }

    public record AssignRolesRequest(List<string> Roles);
    public record RemoveRolesRequest(List<string> Roles);
}

