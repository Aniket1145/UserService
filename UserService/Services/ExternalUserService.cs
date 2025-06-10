using UserService.Client;
using UserService.Models;

namespace UserService.Services
{
    public class ExternalUserService : IExternalUserService
    {
        private readonly IReqResApiClient _apiClient;

        public ExternalUserService(IReqResApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var dto = await _apiClient.GetUserByIdAsync(userId);
            if (dto == null) return null;
            return MapToUser(dto);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var dtos = await _apiClient.GetAllUsersAsync();
            return dtos.Select(MapToUser);
        }

        private static User MapToUser(UserDto dto)
        {
            return new User
            {
                Id = dto.Id,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Avatar = dto.Avatar
            };
        }
    }
}