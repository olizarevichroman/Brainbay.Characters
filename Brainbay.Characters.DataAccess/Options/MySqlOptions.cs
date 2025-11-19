namespace Brainbay.Characters.DataAccess.Options;

public sealed class MySqlOptions
{
    public string Database { get; set; } = null!;

    public string Server { get; set; } = null!;

    public uint Port { get; set; } = 3306;

    public string UserId { get; set; } = "root";
    
    public string Password { get; set; } = "password";
}