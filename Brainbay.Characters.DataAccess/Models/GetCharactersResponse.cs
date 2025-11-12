namespace Brainbay.Characters.DataAccess.Models;

public sealed class GetCharactersResponse(
    IReadOnlyList<object> characters,
    DataSource dataSource)
{
    public IReadOnlyList<object> Characters { get; } = characters;

    public DataSource DataSource { get; } = dataSource;
}