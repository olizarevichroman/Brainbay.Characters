using System.Text.Json.Serialization;

namespace Brainbay.Characters.Integrations.RickAndMorty.Models;

internal sealed class CharacterPageDto
{
    [JsonPropertyName("info")]
    public required Info Information { get; init; }
    
    [JsonPropertyName("results")]
    public required IReadOnlyList<CharacterDto> Characters { get; init; }
    
    
    public sealed class Info
    {
        public required Uri? Next { get; init; }

        public required int Pages { get; init; }
        
        public required int Count { get; init; }
    }
}