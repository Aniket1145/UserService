using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using UserService.Client;
using UserService.Models;

public class ReqResApiClientTests
{
    private HttpClient CreateMockHttpClient(HttpStatusCode statusCode, string content)
    {
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content)
            });

        return new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://mockapi.com/")
        };
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        var fakeUser = new UserDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" };
        var json = JsonSerializer.Serialize(new { data = fakeUser });
        var client = CreateMockHttpClient(HttpStatusCode.OK, json);
        var apiClient = new ReqResApiClient(client);

        var result = await apiClient.GetUserByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Email.Should().Be("john@example.com");
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserNotFound()
    {
        var client = CreateMockHttpClient(HttpStatusCode.NotFound, "");
        var apiClient = new ReqResApiClient(client);

        var result = await apiClient.GetUserByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnAllUsers()
    {
        var userList = new UserListResponseDto
        {
            Page = 1,
            TotalPages = 1,
            Data = new List<UserDto>
            {
                new UserDto { Id = 1, FirstName = "A", LastName = "B", Email = "a@b.com" },
                new UserDto { Id = 2, FirstName = "C", LastName = "D", Email = "c@d.com" }
            }
        };
        var json = JsonSerializer.Serialize(userList);
        var client = CreateMockHttpClient(HttpStatusCode.OK, json);
        var apiClient = new ReqResApiClient(client);

        var result = await apiClient.GetAllUsersAsync();

        result.Should().HaveCount(2);
        result[0].FirstName.Should().Be("A");
        result[1].Email.Should().Be("c@d.com");
    }
}
