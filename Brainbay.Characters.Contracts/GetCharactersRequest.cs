namespace Brainbay.Characters.Contracts;

public sealed class GetCharactersRequest(int skip, int take)
{
    public int Skip { get; } = skip;

    public int Take { get; } = take;
}