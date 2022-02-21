namespace MVC.Models.Requests
{
    public class GetTypesRequest<T>
    {
        public IEnumerable<T> Data { get; set; } = null!;
    }
}
