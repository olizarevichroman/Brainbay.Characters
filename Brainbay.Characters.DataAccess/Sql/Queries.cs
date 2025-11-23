using System.Text;

namespace Brainbay.Characters.DataAccess.Sql;

internal static class Queries
{
    public static readonly string GetCharacters = new StringBuilder()
        .AppendLine("SELECT Id, Name, Species, Status, Gender, CreatedAt, ImageUrl")
        .AppendLine("FROM Characters")
        .AppendLine("ORDER BY Id DESC")
        .AppendLine("LIMIT @Skip, @Take;")
        .AppendLine()
        .AppendLine("SELECT COUNT(1) FROM Characters;")
        .ToString();

    public static readonly string RegisterCharacter = new StringBuilder()
        .AppendLine("INSERT INTO Characters (Name, Species, Status, Gender, CreatedAt, ImageUrl)")
        .AppendLine("VALUES (@Name, @Species, @Status, @Gender, @CreatedAt, @ImageUrl)")
        .AppendLine("ON DUPLICATE KEY UPDATE Status = VALUES(Status), Gender = VALUES(Gender), ImageUrl = VALUES(ImageUrl), Species = VALUES(Species);")
        .AppendLine()
        .AppendLine("SELECT @LAST_INSERT_ID();")
        .ToString();
    
    public const string CleanupCharacters = "TRUNCATE TABLE Characters;";
}
