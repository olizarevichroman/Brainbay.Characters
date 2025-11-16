using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Brainbay.Characters.WebApi.Models;

public sealed class GetCharactersClientRequest
{
    [BindRequired]
    public int PageSize { get; set; } = 10;
    
    public int? LatestId { get; set; }
}