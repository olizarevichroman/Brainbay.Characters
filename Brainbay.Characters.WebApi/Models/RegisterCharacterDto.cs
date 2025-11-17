using System.ComponentModel.DataAnnotations;
using Brainbay.Characters.Contracts;

namespace Brainbay.Characters.WebApi.Models;

public sealed class RegisterCharacterDto(string name, string species, CharacterStatus status, CharacterGender gender, string imageUrl)
{
    [MaxLength(ValidationConstants.CharacterNameMaxLength)]
    public string Name { get; } = name;

    [MaxLength(ValidationConstants.CharacterSpeciesMaxLength)]
    public string Species { get; } = species;

    public CharacterStatus Status { get; } = status;

    public CharacterGender Gender { get; } = gender;

    [MaxLength(ValidationConstants.CharacterImageUrlMaxLength)]
    public string ImageUrl { get; } = imageUrl;
}