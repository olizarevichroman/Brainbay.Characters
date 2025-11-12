using System.Net.Http.Json;
using Brainbay.Characters.Integrations.RickAndMorty.Models;

var baseUri = new Uri("https://rickandmortyapi.com/api/character/");

var httpClient = new HttpClient();
var pageUri = baseUri;

do
{
    var response = await httpClient.GetFromJsonAsync<GetCharacterPageResponse>(pageUri);
    Console.WriteLine($"Page '{pageUri}' was successfully retrieved.'");

    pageUri = response?.Information.Next;
    
} while (pageUri is not null);


Console.ReadKey();