namespace Brainbay.Characters.Integrations.RickAndMorty.Models;

public sealed class CharacterDto(
    int id,
    string name,
    string status,
    string species,
    string type,
    string gender,
    string image,
    string url,
    DateTimeOffset created)
{
    public int Id { get; } = id;

    public string Name { get; } = name;

    public string Status { get; } = status;

    public string Species { get; } = species;

    public string Type { get; } = type;

    public string Gender { get; } = gender;

    public string Image { get; } = image;

    public string Url { get; } = url;

    public DateTimeOffset Created { get; } = created;
}