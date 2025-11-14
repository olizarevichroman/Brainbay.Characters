using System.ComponentModel.DataAnnotations;
using Brainbay.Characters.Contracts;

namespace Brainbay.Characters.WebApi.Models;

public sealed class RegisterCharacterDto(string name, CharacterStatus status, CharacterGender gender)
{
    [MaxLength(ValidationConstants.CharacterNameMaxLength)]
    public string Name { get; } = name;

    public CharacterStatus Status { get; } = status;

    public CharacterGender Gender { get; } = gender;
}