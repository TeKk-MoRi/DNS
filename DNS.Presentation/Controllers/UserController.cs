using DNS.Application.Common;
using DNS.Application.Common.Interfaces;
using DNS.Application.Users.Commands.CreateUser;
using DNS.Application.Users.Queries.GetUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DNS.Presentation.Controllers;

public class UserController : BaseController
{
    //[HttpGet]
    //public async Task<ActionResult<GetUserDto>> GetUser([FromQuery] GetUserQuery query)
    // => await Mediator.Send(query);

    //[HttpPost]
    //public async Task<ActionResult<Guid>> Create(CreateUserCommand command)
    //   => await Mediator.Send(command);
    private readonly IKeycloakService _keycloakService;
    public UserController(IKeycloakService keycloakService)
    {
        _keycloakService = keycloakService;
    }


    [HttpGet("{userId}")]
    public async Task<ActionResult<Result<KeycloakUserDto>>> GetUserById(string userId)
    {
        Result<KeycloakUserDto> result = await _keycloakService.GetUserByIdAsync(userId);

        if (result.IsFailure)
            return BadRequest(Result.Failure(result.Message));

        return Ok(Result<KeycloakUserDto>.Success(result.Data));
    }

    [HttpGet("by-username/{username}")]
    public async Task<ActionResult<Result<KeycloakUserDto>>> GetUserByUsername(string username)
    {
        Result<KeycloakUserDto> result = await _keycloakService.GetUserByUsernameAsync(username);

        if (result.IsFailure)
            return BadRequest(Result.Failure(result.Message));

        return Ok(Result<KeycloakUserDto>.Success(result.Data));
    }

    [HttpGet("by-email/{email}")]
    public async Task<ActionResult<Result<KeycloakUserDto>>> GetUserByEmail(string email)
    {
        Result<KeycloakUserDto> result = await _keycloakService.GetUserByEmailAsync(email);

        if (result.IsFailure)
            return BadRequest(Result.Failure(result.Message));

        return Ok(Result<KeycloakUserDto>.Success(result.Data));
    }

    [HttpPut("{userId}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<Result>> UpdateUser(string userId, [FromBody] UpdateUserRequest request)
    {
        Result<bool> result = await _keycloakService.UpdateUserAsync(
            userId, request.Email, request.FirstName, request.LastName);

        if (result.IsFailure)
            return BadRequest(Result.Failure(result.Message));

        return Ok(Result.Success("User updated successfully"));
    }

    [HttpDelete("{userId}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<Result>> DeleteUser(string userId)
    {
        Result<bool> result = await _keycloakService.DeleteUserAsync(userId);

        if (result.IsFailure)
            return BadRequest(Result.Failure(result.Message));

        return Ok(Result.Success("User deleted successfully"));
    }
}

public record UpdateUserRequest(string Email, string FirstName, string LastName);
