namespace Brainbay.Characters.Integrations.RickAndMorty.Models;

public sealed class GetCharacterPageResponse(
    IReadOnlyList<CharacterDto> characters,
    GetCharactersRequest? nextPageRequest)
{
    public IReadOnlyList<CharacterDto> Characters { get; } = characters;

    public GetCharactersRequest? NextPageRequest { get; } = nextPageRequest;
}