using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using UserService.Client;
using UserService.Models;
using UserService.Services;
using Xunit;

public class ExternalUserServiceTests
{
    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        var mockClient = new Mock<IReqResApiClient>();
        var dto = new UserDto
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com",
            Avatar = "avatar-url"
        };

        mockClient.Setup(c => c.GetUserByIdAsync(1)).ReturnsAsync(dto);
        var service = new ExternalUserService(mockClient.Object);

        var result = await service.GetUserByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.FirstName.Should().Be("Jane");
        result.Email.Should().Be("jane@example.com");
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        var mockClient = new Mock<IReqResApiClient>();
        mockClient.Setup(c => c.GetUserByIdAsync(999)).ReturnsAsync((UserDto?)null);
        var service = new ExternalUserService(mockClient.Object);

        var result = await service.GetUserByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnMappedUsers()
    {
        var mockClient = new Mock<IReqResApiClient>();
        var dtos = new List<UserDto>
        {
            new UserDto { Id = 1, FirstName = "A", LastName = "B", Email = "a@b.com", Avatar = "avatar1" },
            new UserDto { Id = 2, FirstName = "C", LastName = "D", Email = "c@d.com", Avatar = "avatar2" }
        };

        mockClient.Setup(c => c.GetAllUsersAsync()).ReturnsAsync(dtos);
        var service = new ExternalUserService(mockClient.Object);

        var result = (await service.GetAllUsersAsync()).ToList();

        result.Should().HaveCount(2);
        result[0].FirstName.Should().Be("A");
        result[1].Email.Should().Be("c@d.com");
    }
}
