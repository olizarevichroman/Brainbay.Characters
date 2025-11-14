using Brainbay.Characters.Domain;

namespace Brainbay.Characters.WebApi.Models;

public sealed class RegisterCharacterDto(string name, CharacterStatus status, CharacterGender gender)
{
    public string Name { get; } = name;

    public CharacterStatus Status { get; } = status;

    public CharacterGender Gender { get; } = gender;
}