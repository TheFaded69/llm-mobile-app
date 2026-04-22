namespace GameModes.Contracts.V1;

public class PagedSetCardResponse
{
    public List<SetCard> Items { get; set; } = [];
    public PaginationMeta Pagination { get; set; } = new();
}
