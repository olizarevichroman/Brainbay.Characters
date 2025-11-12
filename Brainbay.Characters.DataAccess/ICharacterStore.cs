namespace Brainbay.Characters.DataAccess;

public interface ICharacterStore
{
    Task RegisterCharactersAsync();

    Task GetCharactersAsync();
}