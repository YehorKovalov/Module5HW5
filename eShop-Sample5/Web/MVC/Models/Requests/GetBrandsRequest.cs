namespace MVC.Models.Requests
{
    public class GetBrandsRequest<T>
    {
        public IEnumerable<T> Data { get; set; } = null!;
    }
}
