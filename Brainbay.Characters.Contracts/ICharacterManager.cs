namespace Brainbay.Characters.Contracts;

public interface ICharacterManager
{
    Task<GetCharactersResponse> GetCharactersAsync();
    
    Task RegisterCharacterAsync(RegisterCharacterRequest request);
}