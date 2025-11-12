namespace Brainbay.Characters.WebApi.Extensions;

internal static class HeaderDictionaryExtensions
{
    public static void AppendFromDatabase(this IHeaderDictionary headers)
    {
        headers.Append(Headers.FromDatabase, value: "1");
    }
}