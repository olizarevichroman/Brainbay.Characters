namespace Brainbay.Characters.Contracts;

public sealed class RegisterCharacterRequest(
    string name,
    CharacterStatus status,
    CharacterGender gender)
{
    public string Name { get; } = name;

    public CharacterStatus Status { get; } = status;

    public CharacterGender Gender { get; } = gender;
}