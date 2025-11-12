using System.Data.Common;

namespace Brainbay.Characters.DataAccess;

internal sealed class SqlCharacterStore : ICharacterStore
{
    public Task RegisterCharactersAsync()
    {
        throw new NotImplementedException();
    }

    public async Task GetCharactersAsync()
    {
        
    }
}