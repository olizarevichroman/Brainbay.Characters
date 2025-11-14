namespace Brainbay.Characters.Contracts;

public sealed class Character(
    string name,
    CharacterStatus status,
    CharacterGender gender,
    DateTimeOffset created)
{
    public string Name { get; } = name;

    public CharacterStatus Status { get; } = status;

    public CharacterGender Gender { get; } = gender;

    public DateTimeOffset Created { get; } = created;
}