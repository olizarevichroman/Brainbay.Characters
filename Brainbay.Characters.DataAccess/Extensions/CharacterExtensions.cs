using Brainbay.Characters.Contracts;
using Brainbay.Characters.DataAccess.Models;

namespace Brainbay.Characters.DataAccess.Extensions;

internal static class CharacterExtensions
{
    public static CharacterDto ToDto(this Character character) => new(
        character.Id,
        character.Name,
        character.Species,
        character.Status,
        character.Gender,
        character.Created,
        character.ImageUrl.ToString());
}