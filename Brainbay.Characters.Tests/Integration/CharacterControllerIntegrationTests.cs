using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bogus;
using Brainbay.Characters.Contracts;
using Brainbay.Characters.WebApi.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Brainbay.Characters.Tests.Integration;

public sealed class CharacterControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        Converters =
        {
            new JsonStringEnumConverter(),
        },
    };

    private readonly HttpClient _client;
    private readonly Faker<RegisterCharacterDto> _registerDtoFaker;

    public CharacterControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();

        _registerDtoFaker = new Faker<RegisterCharacterDto>()
            .RuleFor(x => x.Name, f => f.Person.FullName)
            .RuleFor(x => x.Species, f => f.Lorem.Word())
            .RuleFor(x => x.Status, f => f.PickRandom<CharacterStatus>())
            .RuleFor(x => x.Gender, f => f.PickRandom<CharacterGender>())
            .RuleFor(x => x.ImageUrl, f => f.Internet.Url());
    }

    [Fact]
    public async Task GetCharacters_RequestIsValid_ShouldReturnOk()
    {
        // Arrange
        const int skip = 0;
        const int take = 10;
        var requestUri = $"/api/characters?skip={skip}&take={take}";

        // Act
        var response = await _client.GetAsync(requestUri, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadFromJsonAsync<GetCharactersResponse>(
            SerializerOptions,
            TestContext.Current.CancellationToken);

        content.Should().NotBeNull();
    }

    [Fact]
    public async Task RegisterCharacter_RequestIsValid_ShouldReturnCreated()
    {
        // Arrange
        var validRequest = _registerDtoFaker.Generate();

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/characters",
            validRequest,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task RegisterCharacter_ShouldReturnBadRequest_WhenImageUrlIsInvalid()
    {
        // Arrange
        var invalidRequest = _registerDtoFaker.Clone()
            .RuleFor(x => x.ImageUrl, _ => "invalid-url-string")
            .Generate();

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/characters",
            invalidRequest,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(
            SerializerOptions,
            cancellationToken: TestContext.Current.CancellationToken);

        problemDetails.Should().NotBeNull();
        problemDetails.Errors.Should().ContainKey(nameof(RegisterCharacterDto.ImageUrl));
    }
}
