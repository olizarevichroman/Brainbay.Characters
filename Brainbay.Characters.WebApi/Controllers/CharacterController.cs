using Brainbay.Characters.DataAccess;
using Brainbay.Characters.DataAccess.Models;
using Brainbay.Characters.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Brainbay.Characters.WebApi.Controllers;

[ApiController]
[Route("api/characters")]
public sealed class CharacterController(ICharacterStore characterStore) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCharacters()
    {
        var response = await characterStore.GetCharactersAsync();

        if (response.DataSource is DataSource.Database)
        {
            HttpContext.Response.Headers.AppendFromDatabase();
        }

        return Ok();
    }

    [HttpPut("{characterId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RegisterCharacter(int characterId)
    {
        var request = new RegisterCharacterRequest();

        await characterStore.RegisterCharacterAsync(request);
        
        return NoContent();
    }
}