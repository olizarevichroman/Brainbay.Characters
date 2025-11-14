namespace Brainbay.Characters.Domain;

public interface ICharacterManager
{
    Task<GetCharactersResponse> GetCharactersAsync();
    
    Task RegisterCharacterAsync(RegisterCharacterRequest request);
}