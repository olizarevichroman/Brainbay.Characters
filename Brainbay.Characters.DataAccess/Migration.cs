namespace Brainbay.Characters.DataAccess;

public sealed class Migration(string name, string query)
{
    public string Name { get; } = name;

    public string Query { get; } = query;
}