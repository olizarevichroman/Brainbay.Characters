using Brainbay.Characters.Domain;

namespace Brainbay.Characters.DataAccess.Models;

public sealed class GetCharactersResponse(IReadOnlyList<Character> characters, DataSource dataSource)
{
    public IReadOnlyList<Character> Characters { get; } = characters;

    public DataSource DataSource { get; } = dataSource;
}