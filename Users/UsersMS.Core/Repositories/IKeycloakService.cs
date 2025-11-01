using System.Threading.Tasks;
using System.Text.Json;

namespace UsersMS.Core.Repositories
{
    public interface IKeycloakService
    {
        Task<string> GetAdminTokenAsync();

        Task CreateUserAsync(object userDto, string token);

        Task AssignRoleAsync(string username, string role, string token);

        Task DeleteUserAsync(string username, string token);

        Task DisableUserAsync(string username, string token);

        Task UpdateUserAsync(string userId, object userDto, string token);

        Task<JsonElement?> GetUserAsync(string username, string token);
    }
}
