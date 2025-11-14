using System.Net.Http.Json;
using Brainbay.Characters.Integrations.RickAndMorty.Models;

namespace Brainbay.Characters.Integrations.RickAndMorty.Services;

public sealed class RickAndMortyApiClient(HttpClient httpClient) : IRickAndMortyApiClient
{
    private const string BaseUrl = "https://rickandmortyapi.com/api";

    public async Task<GetCharacterPageResponse?> GetCharactersAsync(GetCharactersRequest request)
    {
        const string requestUri = $"{BaseUrl}/character";
        const string pageQueryName = "page";

        try
        {
            var response = await httpClient.GetFromJsonAsync<CharacterPageDto>(requestUri);

            if (response is null)
            {
                return null;
            }

            var nextPageUri = response?.Information.Next;
            var query = nextPageUri is null
                ? []
                : System.Web.HttpUtility.ParseQueryString(nextPageUri.Query);
            
            var filters = query.AllKeys.Select(x => KeyValuePair.Create(x, query[x])).ToDictionary();
            
            var nextPage = filters.GetValueOrDefault(pageQueryName, defaultValue: 1);
            var nextPageRequest = GetNextPage();

            return new GetCharacterPageResponse(response.Characters, nextPageRequest);
        }
        catch
        {
            return null;
        }

        static GetCharactersRequest? GetNextPage()
        {
            
        }
    }
}