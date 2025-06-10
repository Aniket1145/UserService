using System.Text.Json;
using UserService.Models;

namespace UserService.Client
{
    public class ReqResApiClient : IReqResApiClient
    {
        private readonly HttpClient _httpClient;

        public ReqResApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"users/{userId}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("data", out var dataElement))
                {
                    return JsonSerializer.Deserialize<UserDto>(dataElement.GetRawText());
                }
                return null;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Network error while fetching user by ID: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Deserialization error: {ex.Message}", ex);
            }
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = new List<UserDto>();
            int page = 1;
            int totalPages = 1;
            try
            {
                do
                {
                    var response = await _httpClient.GetAsync($"users?page={page}");
                    response.EnsureSuccessStatusCode();
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<UserListResponseDto>(json);
                    if (result != null)
                    {
                        users.AddRange(result.Data);
                        totalPages = result.TotalPages;
                    }
                    page++;
                } while (page <= totalPages);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Network error while fetching users: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Deserialization error: {ex.Message}", ex);
            }
            return users;
        }
    }
}