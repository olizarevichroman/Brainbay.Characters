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

        int skip = 5;
        int take = 7;

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
        var characters = CharacterFaker.Create().Generate(3);

        var fallback = new Mock<ICharacterStore>();

        fallback
            .Setup(x => x.GetCharactersAsync(It.IsAny<GetCharactersRequest>()))
            .ReturnsAsync((GetCharactersRequest req) =>
            {
                var page = characters.Skip(req.Skip).Take(req.Take).ToList();
                return new GetCharactersResponse(page, DataSource.Database, characters.Count);
            });

        fallback
            .Setup(x => x.RegisterCharacterAsync(It.IsAny<RegisterCharacterRequest>()))
            .Callback(() => characters.Add(CharacterFaker.Create().Generate()))
            .Returns(Task.CompletedTask);

        var store = new InMemoryCharacterStore(fallback.Object, _cache, _options);

        var getCharactersRequest = new GetCharactersRequest(0, 100);
        var first = await store.GetCharactersAsync(getCharactersRequest);

        // Act
        var registerRequest = RegisterCharacterRequestFaker.Create().Generate();

        await store.RegisterCharacterAsync(registerRequest);
        var second = await store.GetCharactersAsync(getCharactersRequest);

        // Assert
        fallback.Verify(x => x.RegisterCharacterAsync(It.IsAny<RegisterCharacterRequest>()), Times.Once);

        fallback.Verify(x => x.GetCharactersAsync(It.IsAny<GetCharactersRequest>()), Times.Exactly(2));

        first.TotalCount.Should().Be(3);
        second.TotalCount.Should().Be(4);

        second.Characters.Should().HaveCount(4);
    }
}