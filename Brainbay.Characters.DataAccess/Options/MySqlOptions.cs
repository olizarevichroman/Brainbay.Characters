namespace Brainbay.Characters.DataAccess.Options;

public sealed class MySqlOptions
{
    public string Database { get; init; } = "default";

    public string Server { get; init; } = null!;

    public uint Port { get; init; } = 3306;

    public string UserId { get; init; } = "root";
    
    public string Password { get; init; } = "password";
}