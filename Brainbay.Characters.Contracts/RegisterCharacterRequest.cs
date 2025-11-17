namespace Brainbay.Characters.Contracts;

public sealed class RegisterCharacterRequest(
    string name,
    string species,
    CharacterStatus status,
    CharacterGender gender,
    Uri imageUrl)
{
    public string Name { get; } = name;
    
    public string Species { get; } = species;

    public CharacterStatus Status { get; } = status;

    public CharacterGender Gender { get; } = gender;

    public Uri ImageUrl { get; } = imageUrl;
}