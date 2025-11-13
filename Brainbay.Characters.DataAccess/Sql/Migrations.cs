using System.Text;

namespace Brainbay.Characters.DataAccess.Sql;

public static class Migrations
{
    private static readonly string CreateCharactersTable = new StringBuilder()
        .AppendLine("CREATE TABLE IF NOT EXISTS Characters (")
        .AppendLine("    Id BIGINT UNSIGNED AUTO_INCREMENT,")
        .AppendLine("    Name VARCHAR(100) UNIQUE,")
        .AppendLine("    PRIMARY KEY (Id)")
        .AppendLine(");")
        .ToString();

    public static IReadOnlyList<Migration> List =>
    [
        new("Create 'Characters' table.", CreateCharactersTable),
    ];
}