using MVC.Dtos;
using MVC.Models.Enums;
using MVC.Models.Requests;
using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Services;

public class CatalogService : ICatalogService
{
    private readonly IOptions<AppSettings> _settings;
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<CatalogService> _logger;

    public CatalogService(
        IHttpClientService httpClient,
        ILogger<CatalogService> logger,
        IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;
    }

    public async Task<Catalog> GetCatalogItems(int page, int take, int? brand, int? type)
    {
        var filters = new Dictionary<CatalogTypeFilter, int>();

        if (brand.HasValue)
        {
            filters.Add(CatalogTypeFilter.Brand, brand.Value);
        }
        
        if (type.HasValue)
        {
            filters.Add(CatalogTypeFilter.Type, type.Value);
        }
        
        var result = await _httpClient
            .SendAsync<Catalog, PaginatedItemsRequest<CatalogTypeFilter>>($"{_settings.Value.CatalogUrl}/items",
           HttpMethod.Post, 
           new PaginatedItemsRequest<CatalogTypeFilter>()
            {
                PageIndex = page,
                PageSize = take,
                Filters = filters
            });

        return result;
    }

    public async Task<IEnumerable<SelectListItem>> GetBrands()
    {
        var response = await _httpClient
            .SendAsync<GetBrandsRequest<CatalogBrand>>($"{_settings.Value.CatalogUrl}/brands", HttpMethod.Get);
        if (response == null)
        {
            return Enumerable.Empty<SelectListItem>();
        }

        var brands = response.Data;
        var list = new List<SelectListItem>();
        foreach (var brand in brands)
        {
            list.Add(new SelectListItem
            {
                Value = brand.Id.ToString(),
                Text = brand.Brand
            });
        }

        return list;
    }

    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
        var response = await _httpClient
            .SendAsync<GetTypesRequest<CatalogType>>($"{_settings.Value.CatalogUrl}/types", HttpMethod.Get);
        if (response == null)
        {
            return Enumerable.Empty<SelectListItem>();
        }

        var types = response.Data;
        var list = new List<SelectListItem>();
        foreach (var type in types)
        {
            list.Add(new SelectListItem
            {
                Value = type.Id.ToString(),
                Text = type.Type
            });
        }

        return list;
    }
}
