using Brainbay.Characters.Contracts;
using Brainbay.Characters.DataAccess;
using Brainbay.Characters.DataAccess.Options;
using Brainbay.Characters.Tests.Fakers;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;

namespace Brainbay.Characters.Tests.Unit;

public sealed class InMemoryCharactersStoreUnitTests
{
    private readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private readonly IOptions<CharacterCacheOptions> _options = Options.Create(new CharacterCacheOptions
    {
        MaxCacheDuration = TimeSpan.FromMinutes(2),
    });

    [Fact]
    public async Task GetCharactersAsync_CachesResults_AfterFirstCall()
    {
        var characters = CharacterFaker.Create().Generate(count: 10);

        var fallback = new Mock<ICharacterStore>();

        fallback
            .Setup(x => x.GetCharactersAsync(It.IsAny<GetCharactersRequest>()))
            .ReturnsAsync((GetCharactersRequest req) =>
            {
                var page = characters.Skip(req.Skip).Take(req.Take).ToList();
                return new GetCharactersResponse(page, DataSource.Database, characters.Count);
            });

        var store = new InMemoryCharacterStore(fallback.Object, _cache, _options);
        var request = new GetCharactersRequest(skip: 0, take: 5);

        var first = await store.GetCharactersAsync(request);
        var second = await store.GetCharactersAsync(request);

        fallback.Verify(x => x.GetCharactersAsync(It.IsAny<GetCharactersRequest>()), Times.Once);

        first.DataSource.Should().Be(DataSource.Database);
        first.TotalCount.Should().Be(characters.Count);
        
        second.DataSource.Should().Be(DataSource.Cache);
        second.TotalCount.Should().Be(characters.Count);
    }

    [Fact]
    public async Task GetCharactersAsync_AppliesSkipAndTake()
    {
        // Arrange
        var characters = CharacterFaker.Create().Generate(count: 20);

        var fallback = new Mock<ICharacterStore>();
        fallback
            .Setup(x => x.GetCharactersAsync(It.IsAny<GetCharactersRequest>()))
            .ReturnsAsync((GetCharactersRequest req) =>
            {
                var page = characters.Skip(req.Skip).Take(req.Take).ToList();

                return new GetCharactersResponse(page, DataSource.Database, characters.Count);
            });

        var store = new InMemoryCharacterStore(fallback.Object, _cache, _options);

        const int skip = 5;
        const int take = 7;

        var request = new GetCharactersRequest(skip, take);

        // Act
        var result = await store.GetCharactersAsync(request);

        // Assert
        result.Characters.Should().HaveCount(take);

        var expected = characters.Skip(skip).Take(take).ToList();

        result.Characters.Should().BeEquivalentTo(
            expected,
            options => options.WithStrictOrdering(),
            "The order and the characters themselves must match the original list, taking Skip/Take into account.");

        result.TotalCount.Should().Be(characters.Count);
        result.DataSource.Should().Be(DataSource.Database);
    }

    [Fact]
    public async Task RegisterCharacterAsync_ClearsCache_AndTriggersFreshFetch()
    {
        // Arrange
        const int initialCharacterCount = 10;
        var characters = CharacterFaker.Create().Generate(initialCharacterCount);
        var expectedDate = DateTimeOffset.UtcNow;

        const int expectedId = 42;

        var fallbackStore = new Mock<ICharacterStore>();

        fallbackStore
            .Setup(x => x.GetCharactersAsync(It.IsAny<GetCharactersRequest>()))
            .ReturnsAsync((GetCharactersRequest req) =>
            {
                var page = characters.Skip(req.Skip).Take(req.Take).ToList();

                return new GetCharactersResponse(page, DataSource.Database, characters.Count);
            });

        fallbackStore
            .Setup(x => x.RegisterCharacterAsync(
                It.IsAny<RegisterCharacterRequest>(),
                It.IsAny<DateTimeOffset>()))
            .ReturnsAsync((RegisterCharacterRequest x, DateTimeOffset now) =>
            {
                var newCharacter = new Character(expectedId, x.Name, x.Species, x.Status, x.Gender, now, x.ImageUrl);
                characters.Add(newCharacter);

                return expectedId;
            });

        var inMemoryStore = new InMemoryCharacterStore(fallbackStore.Object, _cache, _options);

        var getCharactersRequest = new GetCharactersRequest(0, 100);
        var first = await inMemoryStore.GetCharactersAsync(getCharactersRequest);

        // Act
        var registerRequest = RegisterCharacterRequestFaker.Create().Generate();
        await inMemoryStore.RegisterCharacterAsync(registerRequest, expectedDate);

        var second = await inMemoryStore.GetCharactersAsync(getCharactersRequest);

        // Assert
        fallbackStore.Verify(x => x.RegisterCharacterAsync(
            It.IsAny<RegisterCharacterRequest>(),
            It.IsAny<DateTimeOffset>()), Times.Once);

        fallbackStore.Verify(x => x.GetCharactersAsync(It.IsAny<GetCharactersRequest>()), Times.Exactly(2));

        first.TotalCount.Should().Be(initialCharacterCount);
        second.TotalCount.Should().Be(initialCharacterCount + 1);
        second.Characters.Should().HaveCount(initialCharacterCount + 1);

        var addedCharacter = second.Characters.Last();
        addedCharacter.Id.Should().Be(expectedId);
        addedCharacter.Name.Should().Be(registerRequest.Name);
        addedCharacter.Species.Should().Be(registerRequest.Species);
        addedCharacter.Status.Should().Be(registerRequest.Status);
        addedCharacter.Gender.Should().Be(registerRequest.Gender);
        addedCharacter.ImageUrl.Should().Be(registerRequest.ImageUrl);
        addedCharacter.Created.Should().Be(expectedDate);

        second.DataSource.Should().Be(DataSource.Database);

        fallbackStore.Verify(x => x.RegisterCharacterAsync(
            It.Is<RegisterCharacterRequest>(r => r.Name == registerRequest.Name),
            expectedDate), Times.Once);
    }
}
