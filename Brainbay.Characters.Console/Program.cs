using System.Net.Http.Json;
using Brainbay.Characters.Domain;
using Brainbay.Characters.Integrations.RickAndMorty.Models;
using Microsoft.Extensions.Logging;

var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<Program>();

var baseUri = new Uri($"https://rickandmortyapi.com/api/character?status={CharacterStatus.Alive}");
var pageUri = baseUri;

var httpClient = new HttpClient();

do
{
    var response = await httpClient.GetFromJsonAsync<GetCharacterPageResponse>(pageUri);

    if (response is null)
    {
        logger.LogWarning("Received a null response from {PageName}. Stopping application.", pageUri);

        return;
    }
    
    logger.LogInformation("Page '{PageUri}' was successfully retrieved.", pageUri);

    pageUri = response.Information.Next;
    
} while (pageUri is not null);