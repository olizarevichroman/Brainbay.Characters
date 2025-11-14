namespace Brainbay.Characters.Integrations.RickAndMorty.Models;

public sealed class GetCharactersRequest(IReadOnlyDictionary<string, string> filters)
{
    public IReadOnlyDictionary<string, string> Filters { get; } = filters;
}