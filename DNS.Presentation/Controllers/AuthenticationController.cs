using DNS.Application.Authentication.Commands.UserRegister;
using DNS.Application.Authentication.Queries.Login;
using DNS.Application.Common;
using DNS.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DNS.Presentation.Controllers;

public class AuthenticationController : BaseController
{

    private readonly IKeycloakService _keycloakService;

    public AuthenticationController(IKeycloakService keycloakService)
    {
        _keycloakService = keycloakService;
    }
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<Result<KeycloakTokenResponse>>> Login([FromBody] LoginRequest request)
    {
        Result<KeycloakTokenResponse> result = await _keycloakService.LoginAsync(request.Username, request.Password);

        if (result.IsFailure)
            return BadRequest(Result.Failure(result.Message));

        return Ok(Result<KeycloakTokenResponse>.Success(result.Data));
    }

    [HttpPost]
    public async Task<ActionResult<Result>> Logout([FromBody] LogoutRequest request)
    {
        Result<bool> result = await _keycloakService.LogoutAsync(request.RefreshToken);

        if (result.IsFailure)
            return BadRequest(Result.Failure(result.Message));

        return Ok(Result.Success("Logged out successfully"));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<Result<string>>> Register([FromBody] RegisterRequest request)
    {
        Result<string> result = await _keycloakService.CreateUserAsync(
            request.Username,
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password);

        if (result.IsFailure)
            return BadRequest(Result.Failure(result.Message));

        return Ok(Result<string>.Success(result.Data, "User created successfully"));
    }
}

// DTOs for Authentication
public record LoginRequest(string Username, string Password);
public record LogoutRequest(string RefreshToken);
public record RegisterRequest(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string Password);


