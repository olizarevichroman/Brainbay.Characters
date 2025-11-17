using Brainbay.Characters.Contracts;

namespace Brainbay.Characters.DataAccess.Sql;

public static class Migrations
{
    private static readonly string CreateCharactersTable = $"""
        CREATE TABLE IF NOT EXISTS Characters (
            Id BIGINT UNSIGNED AUTO_INCREMENT NOT NULL,
            Name VARCHAR({ValidationConstants.CharacterNameMaxLength}) NOT NULL,
            Species VARCHAR({ValidationConstants.CharacterSpeciesMaxLength}) NOT NULL,
            Status TINYINT UNSIGNED NOT NULL,
            Gender TINYINT UNSIGNED NOT NULL,
            CreatedAt DATETIME(3) NOT NULL,
            ImageUrl VARCHAR({ValidationConstants.CharacterImageUrlMaxLength}) NOT NULL,
            PRIMARY KEY (Id)
        );
    """;

    public static IReadOnlyList<Migration> List =>
    [
        new("Create `Characters` table.", CreateCharactersTable),
    ];
}