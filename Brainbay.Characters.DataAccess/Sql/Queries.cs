using System.Text;

namespace Brainbay.Characters.DataAccess.Sql;

internal static class Queries
{
    public static readonly string GetCharacters = new StringBuilder()
        .AppendLine("SELECT Id, Name, Status, Gender, CreatedAt, ImageUrl")
        .AppendLine("FROM Characters")
        .AppendLine("WHERE Id > @LatestId")
        .AppendLine("ORDER BY Id ASC")
        .AppendLine("LIMIT @Take;")
        .ToString();
    
    public static readonly string RegisterCharacter = new StringBuilder()
        .AppendLine("INSERT @Name, @Status")
        .AppendLine("INTO Characters;")
        .ToString();
    
    public static readonly string RegisterCharacterBatch = new StringBuilder()
        .AppendLine("INSERT @Id, @Name, @Status")
        .AppendLine("INTO Characters;")
        .ToString();
    
    public const string CleanupCharacters = "TRUNCATE TABLE Characters;";
}