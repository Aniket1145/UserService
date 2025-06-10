using UserService.Models;

namespace UserService.Services
{
    public interface IExternalUserService
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}