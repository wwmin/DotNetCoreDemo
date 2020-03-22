using JwtDemo.Authorization.Secret.Dto;

namespace JwtDemo.Authorization.Secret
{
    public interface ISecretService
    {
        UserDto GetCurrentUserAsync(string account, string password);
    }
}