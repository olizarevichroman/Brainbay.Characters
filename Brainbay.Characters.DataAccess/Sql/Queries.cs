using System.Text;

namespace Brainbay.Characters.DataAccess.Sql;

internal static class Queries
{
    public static readonly string GetCharacters = new StringBuilder()
        .AppendLine("SELECT Name, Status")
        .AppendLine("FROM Characters")
        .AppendLine("WHERE Id > @LatestId")
        .AppendLine("LIMIT @Take;")
        .ToString();
    
    public static readonly string RegisterCharacter = new StringBuilder()
        .AppendLine("INSERT @Name, @Status")
        .AppendLine("INTO Characters;")
        .ToString();
}