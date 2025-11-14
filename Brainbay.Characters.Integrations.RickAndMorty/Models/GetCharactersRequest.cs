namespace Brainbay.Characters.Integrations.RickAndMorty.Models;

public sealed class GetCharactersRequest(int page, IReadOnlyDictionary<string, string> filters)
{
    public int Page { get; } = page;

    public IReadOnlyDictionary<string, string> Filters { get; } = filters;
}