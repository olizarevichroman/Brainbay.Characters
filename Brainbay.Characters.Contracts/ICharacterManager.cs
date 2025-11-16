namespace Brainbay.Characters.Contracts;

public interface ICharacterManager
{
    Task<GetCharactersResponse> GetCharactersAsync(GetCharactersRequest request);
    
    Task RegisterCharacterAsync(RegisterCharacterRequest request);
}