namespace Catalog.Host.Models.Requests.ItemRequests;

public class PaginatedItemsRequest<T>
    where T : notnull
{
    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public Dictionary<T, int>? Filters { get; set; }
}