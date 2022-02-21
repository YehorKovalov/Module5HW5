namespace Catalog.Host.Models.Response.BrandResponses
{
    public class GetBrandsResponse<T>
    {
        public IEnumerable<T> Data { get; set; } = null!;
    }
}
