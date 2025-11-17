namespace Brainbay.Characters.Contracts;

public sealed class GetCharactersResponse(IReadOnlyList<Character> characters, DataSource dataSource, int totalCount)
{
    public IReadOnlyList<Character> Characters { get; } = characters;

    public DataSource DataSource { get; } = dataSource;

    public int TotalCount { get; } = totalCount;
}