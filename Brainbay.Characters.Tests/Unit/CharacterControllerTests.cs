using Bogus;
using Brainbay.Characters.Contracts;
using Brainbay.Characters.WebApi;
using Brainbay.Characters.WebApi.Controllers;
using Brainbay.Characters.WebApi.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Brainbay.Characters.Tests.Unit;

public sealed class CharacterControllerUnitTests
{
    private readonly Mock<ICharacterManager> _managerMock;
    private readonly CharacterController _controller;
    private readonly DefaultHttpContext _httpContext;

    public CharacterControllerUnitTests()
    {
        _managerMock = new Mock<ICharacterManager>();
        _controller = new CharacterController(_managerMock.Object);

        _httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = _httpContext
        };
    }

    [Fact]
    public async Task GetCharacters_ShouldReturnOk_AndSetHeader_WhenFromDatabase()
    {
        // Arrange
        var clientRequest = new GetCharactersClientRequest
        {
            Skip = 0,
            Take = 10,
        };

        _managerMock
            .Setup(x => x.GetCharactersAsync(It.IsAny<GetCharactersRequest>()))
            .ReturnsAsync(new GetCharactersResponse([], DataSource.Database, 0));

        // Act
        var result = await _controller.GetCharacters(clientRequest);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeOfType<GetCharactersResponse>();

        _httpContext.Response.Headers.Should().ContainKey(Headers.FromDatabase);
        _httpContext.Response.Headers[Headers.FromDatabase].ToString().Should().Be("1");
    }

    [Fact]
    public async Task GetCharacters_ShouldNotSetHeader_WhenFromCache()
    {
        // Arrange
        var clientRequest = new GetCharactersClientRequest
        {
            Skip = 0,
            Take = 10,
        };

        _managerMock
            .Setup(x => x.GetCharactersAsync(It.IsAny<GetCharactersRequest>()))
            .ReturnsAsync(new GetCharactersResponse([], DataSource.Cache, 0));

        // Act
        await _controller.GetCharacters(clientRequest);

        // Assert
        _httpContext.Response.Headers.Should().NotContainKey(Headers.FromDatabase);
    }

    [Fact]
    public async Task RegisterCharacter_ShouldReturnValidationProblem_WhenUrlIsInvalid()
    {
        // Arrange
        var dto = new RegisterCharacterDto(
            "Rick", 
            "Human", 
            CharacterStatus.Alive, 
            CharacterGender.Male, 
            "invalid-url-string"); 

        // Act
        var result = await _controller.RegisterCharacter(dto);

        // Assert
        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;

        var problemDetails = objectResult.Value.Should().BeOfType<ValidationProblemDetails>().Subject;
        problemDetails.Errors.Should().ContainKey(nameof(dto.ImageUrl));
        
        _controller.ModelState.IsValid.Should().BeFalse();
        _controller.ModelState.Should().ContainKey(nameof(dto.ImageUrl));
    }

    [Fact]
    public async Task RegisterCharacter_ShouldCallManager_WhenValid()
    {
        // Arrange
        var faker = new Faker();
        var dto = new RegisterCharacterDto(
            faker.Name.FirstName(), 
            faker.Lorem.Word(), 
            CharacterStatus.Alive, 
            faker.Random.Enum<CharacterGender>(), 
            faker.Internet.Url());

        // Act
        var result = await _controller.RegisterCharacter(dto);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _managerMock.Verify(x => x.RegisterCharacterAsync(
            It.IsAny<RegisterCharacterRequest>()),
            Times.Once);
    }
}