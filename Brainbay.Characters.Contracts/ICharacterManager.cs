namespace Brainbay.Characters.Contracts;

public interface ICharacterManager
{
    Task<GetCharactersResponse> GetCharactersAsync(GetCharactersRequest request);
    
    Task<Character> RegisterCharacterAsync(RegisterCharacterRequest request);
}
