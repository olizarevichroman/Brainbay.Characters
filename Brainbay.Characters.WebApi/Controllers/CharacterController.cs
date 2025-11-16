using Brainbay.Characters.Contracts;
using Brainbay.Characters.WebApi.Extensions;
using Brainbay.Characters.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Brainbay.Characters.WebApi.Controllers;

[ApiController]
[Route("api/characters")]
public sealed class CharacterController(ICharacterManager characterManager) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<GetCharactersResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCharacters([FromQuery] GetCharactersClientRequest clientRequest)
    {
        var request = new GetCharactersRequest(clientRequest.PageSize, clientRequest.LatestId);
        var response = await characterManager.GetCharactersAsync(request);

        if (response.DataSource is DataSource.Database)
        {
            HttpContext.Response.Headers.AppendFromDatabase();
        }

        return Ok(response);
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