using System.ComponentModel.DataAnnotations;
using Brainbay.Characters.Contracts;

namespace Brainbay.Characters.WebApi.Models;

public sealed class RegisterCharacterDto
{
    [MaxLength(ValidationConstants.CharacterNameMaxLength)]
    public string Name { get; set; }

    [MaxLength(ValidationConstants.CharacterSpeciesMaxLength)]
    public string Species { get; set; }

    public CharacterStatus Status { get; set; }

    public CharacterGender Gender { get; set; }

    [MaxLength(ValidationConstants.CharacterImageUrlMaxLength)]
    public string ImageUrl { get; set; }
}