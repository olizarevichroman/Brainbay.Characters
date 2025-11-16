using Brainbay.Characters.Contracts;

namespace Brainbay.Characters.DataAccess.Models;

public sealed class CharacterDto(
    int id,
    string name,
    CharacterStatus status,
    CharacterGender gender,
    DateTimeOffset createdAt,
    string imageUrl)
{
    public int Id { get; } = id;

    public string Name { get; } = name;

    public CharacterStatus Status { get; private set; } = status;

    public CharacterGender Gender { get; private set; } = gender;

    public DateTimeOffset CreatedAt { get; } = createdAt;

    public string ImageUrl { get; } = imageUrl;
}