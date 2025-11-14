using Brainbay.Characters.Domain;
using Brainbay.Characters.WebApi.Extensions;
using Brainbay.Characters.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Brainbay.Characters.WebApi.Controllers;

[ApiController]
[Route("api/characters")]
public sealed class CharacterController(ICharacterManager characterManager) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCharacters()
    {
        var response = await characterManager.GetCharactersAsync();

        if (response.DataSource is DataSource.Database)
        {
            HttpContext.Response.Headers.AppendFromDatabase();
        }

        return Ok();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RegisterCharacter([FromQuery] RegisterCharacterDto characterDto)
    {
        var request = new RegisterCharacterRequest(characterDto.Name, characterDto.Status, characterDto.Gender);

        await characterManager.RegisterCharacterAsync(request);
        
        return NoContent();
    }
}