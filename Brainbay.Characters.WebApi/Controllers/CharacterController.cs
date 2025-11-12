using Brainbay.Characters.DataAccess;
using Brainbay.Characters.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Brainbay.Characters.WebApi.Controllers;

[ApiController]
[Route("api/characters")]
public sealed class CharacterController(ICharacterStore characterStore) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCharacters()
    {
        HttpContext.Response.Headers.AppendFromDatabase();
        throw new NotImplementedException();
    }

    [HttpPut("{characterId}")]
    public async Task<IActionResult> RegisterCharacter(int characterId)
    {
        throw new NotImplementedException();
    }
}