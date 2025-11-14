using Brainbay.Characters.Domain;

namespace Brainbay.Characters.DataAccess.Models;

public sealed class CharacterDto(
    int id,
    string name,
    CharacterStatus status,
    CharacterGender gender,
    DateTimeOffset createdAt)
{
    public int Id { get; } = id;

    public string Name { get; } = name;

    public CharacterStatus Status { get; private set; } = status;

    public CharacterGender Gender { get; private set; } = gender;

    public DateTimeOffset CreatedAt { get; } = createdAt;
}