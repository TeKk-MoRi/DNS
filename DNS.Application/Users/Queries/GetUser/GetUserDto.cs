using DNS.Domain.Enums;

namespace DNS.Application.Users.Queries.GetUser;

public record GetUserDto(Guid Id, Gender Gender, string Email);
