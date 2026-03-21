namespace Shared.Dtos.User
{
    public record UserResultDto(string DisplayName, string Email, string Token, string UserType = "User");
}
