using ServerAPI.DB;
using ServerAPI.Models;

namespace ServerAPI.Services
{
    public interface IUserService
    {
        Task RegisterAsync(UserRegisterDto dto);
        Task<string> LoginAsync(UserLoginDto dto);
        Task<User> GetProfileAsync(string userId);
        Task AddExperienceAsync(string userId, int exp);
        Task<Resources> GetResourcesAsync(string userId);
        Task AddResourcesAsync(string userId, int gold, int gems);
        Task UseResourcesAsync(string userId, int gold, int gems);
    }
}
