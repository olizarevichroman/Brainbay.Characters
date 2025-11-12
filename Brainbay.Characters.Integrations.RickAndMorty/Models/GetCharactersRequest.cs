namespace Brainbay.Characters.Integrations.RickAndMorty.Models;

public sealed class GetCharactersRequest(
    int page,
    IReadOnlyCollection<string> filters)
{
    public int Page { get; } = page;

    public IReadOnlyCollection<string> Filters { get; } = filters;
}