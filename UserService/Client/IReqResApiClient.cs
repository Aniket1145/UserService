using UserService.Models;

namespace UserService.Client
{
    public interface IReqResApiClient
    {
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<List<UserDto>> GetAllUsersAsync();
    }
}