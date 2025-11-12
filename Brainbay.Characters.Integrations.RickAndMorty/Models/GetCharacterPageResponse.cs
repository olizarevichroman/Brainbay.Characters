using System.Text.Json.Serialization;

namespace Brainbay.Characters.Integrations.RickAndMorty.Models;

public sealed class GetCharacterPageResponse
{
    [JsonPropertyName("info")]
    public required Info Information { get; init; }

    
    public sealed class Info
    {
        public Uri? Next { get; set; }

        public int Pages { get; set; }
    }
}