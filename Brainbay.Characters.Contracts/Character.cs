namespace Brainbay.Characters.Contracts;

public sealed class Character(
    int id,
    string name,
    CharacterStatus status,
    CharacterGender gender,
    DateTimeOffset created,
    Uri imageUrl)
{
    public int Id { get; } = id;

    public string Name { get; } = name;

    public CharacterStatus Status { get; } = status;

    public CharacterGender Gender { get; } = gender;

    public DateTimeOffset Created { get; } = created;

    public Uri ImageUrl { get; } = imageUrl;
}