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
    public async Task GetCharacters_DatabaseDataSource_ShouldReturnOk_AndSetHeader()
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
    public async Task GetCharacters_CachedResult_ShouldNotSetHeader()
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
    public async Task RegisterCharacter_UriIsInvalid_ShouldReturnValidationProblem()
    {
        // Arrange
        var faker = new Faker();
        var dto = new RegisterCharacterDto
        {
            Name = faker.Name.FullName(),
            Species = faker.Lorem.Word(),
            Status = CharacterStatus.Alive,
            Gender = faker.PickRandom<CharacterGender>(),
            ImageUrl = "invalid-url-string",
        };

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
    public async Task RegisterCharacter_RequestIsValid_ShouldCallManager()
    {
        // Arrange
        var faker = new Faker();
        var dto = new RegisterCharacterDto
        {
            Name = faker.Name.FirstName(),
            Species = faker.Lorem.Word(),
            Status = CharacterStatus.Alive,
            Gender = faker.Random.Enum<CharacterGender>(),
            ImageUrl = faker.Internet.Url(),
        };

        var now = TimeProvider.System.GetUtcNow();

        _managerMock
            .Setup(x => x.RegisterCharacterAsync(It.IsAny<RegisterCharacterRequest>()))
            .ReturnsAsync((RegisterCharacterRequest req) => new Character(
                 It.IsAny<int>(),
                req.Name,
                req.Species,
                req.Status,
                req.Gender,
                now,
                req.ImageUrl));

        // Act
        var result = await _controller.RegisterCharacter(dto);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedResult>().Subject;
        var character = createdResult.Value.Should().BeOfType<Character>().Subject;

        character.Name.Should().Be(dto.Name);
        character.Species.Should().Be(dto.Species);
        character.Status.Should().Be(dto.Status);
        character.Gender.Should().Be(dto.Gender);
        character.ImageUrl.Should().Be(new Uri(dto.ImageUrl));
        character.Created.Should().Be(now);
    }
}
