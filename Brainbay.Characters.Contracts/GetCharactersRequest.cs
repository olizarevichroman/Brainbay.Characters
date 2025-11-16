namespace Brainbay.Characters.Contracts;

public sealed class GetCharactersRequest(int pageSize, int? latestId)
{
    public int PageSize { get; } = pageSize;

    public int? LatestId { get; } = latestId;
}