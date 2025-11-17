using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Brainbay.Characters.WebApi.Models;

public sealed class GetCharactersClientRequest
{
    [BindRequired, Range(0, int.MaxValue)]
    public int Skip { get; set; }
    
    [BindRequired, Range(0, int.MaxValue)]
    public int Take { get; set; }
}