namespace Brainbay.Characters.Domain;

public sealed class Character(
    int id,
    string name,
    CharacterStatus status,
    CharacterGender gender,
    DateTimeOffset created)
{
    public int Id { get; } = id;

    public string Name { get; } = name;

    public CharacterStatus Status { get; } = status;

    public CharacterGender Gender { get; } = gender;

    public DateTimeOffset Created { get; } = created;
}