using System.Net.Http.Json;
using System.Text;
using Brainbay.Characters.Integrations.RickAndMorty.Models;

namespace Brainbay.Characters.Integrations.RickAndMorty.Services;

public sealed class RickAndMortyApiClient(HttpClient httpClient) : IRickAndMortyApiClient
{
    private const string BaseUrl = "https://rickandmortyapi.com/api";

    public async Task<GetCharacterPageResponse?> GetCharactersAsync(GetCharactersRequest request)
    {
        var requestUri = BuildRequestUri();

        try
        {
            var response = await httpClient.GetFromJsonAsync<CharacterPageDto>(requestUri);

            if (response is null)
            {
                return null;
            }

            var nextPageUri = response.Information.Next;

            if (nextPageUri is null)
            {
                return new GetCharacterPageResponse(response.Characters, nextPageRequest: null);
            }

            var query = System.Web.HttpUtility.ParseQueryString(nextPageUri.Query);
            
            var filters = query.AllKeys
                .Select(x => KeyValuePair.Create(x!, query[x]!))
                .ToDictionary();

            var nextPageRequest = new GetCharactersRequest(filters);

            return new GetCharacterPageResponse(response.Characters, nextPageRequest);
        }
        catch
        {
            return null;
        }

        string BuildRequestUri()
        {
            var uriBuilder = new StringBuilder(BaseUrl).Append("/character?");

            foreach (var pair in request.Filters)
            {
                uriBuilder.Append(pair.Key).Append('=').Append(pair.Value).Append('&');
            }
            
            return uriBuilder.ToString();
        }
    }
}