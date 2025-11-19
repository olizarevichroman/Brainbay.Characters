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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCharacters([FromQuery] GetCharactersClientRequest clientRequest)
    {
        var request = new GetCharactersRequest(clientRequest.Skip, clientRequest.Take);
        var response = await characterManager.GetCharactersAsync(request);

        if (response.DataSource is DataSource.Database)
        {
            HttpContext.Response.Headers.AppendFromDatabase();
        }

        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterCharacter([FromBody] RegisterCharacterDto characterDto)
    {
        if (!Uri.TryCreate(characterDto.ImageUrl, UriKind.Absolute, out var imageUrl))
        {
            ModelState.AddModelError(nameof(characterDto.ImageUrl), "Invalid image URL");

            return ValidationProblem(ModelState);
        }

        var request = new RegisterCharacterRequest(
            characterDto.Name,
            characterDto.Species,
            characterDto.Status,
            characterDto.Gender,
            imageUrl);

        await characterManager.RegisterCharacterAsync(request);
        
        return NoContent();
    }
}