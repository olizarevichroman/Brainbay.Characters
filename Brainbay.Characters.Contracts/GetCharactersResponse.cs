namespace Brainbay.Characters.Contracts;

public sealed class GetCharactersResponse(IReadOnlyList<Character> characters, DataSource dataSource)
{
    public IReadOnlyList<Character> Characters { get; } = characters;

    public DataSource DataSource { get; } = dataSource;
}