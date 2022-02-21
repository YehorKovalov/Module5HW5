using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Response.BrandResponses;
using Catalog.Host.Models.Response.ItemResponses;
using Catalog.Host.Models.Response.TypeResponses;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedItemsResponse<CatalogItemDto>?> GetCatalogItemsAsync(int pageSize, int pageIndex, Dictionary<CatalogTypeFilter, int>? filters);
    Task<GetItemByIdResponse<CatalogItemDto>> GetCatalogItemByIdAsync(int id);
    Task<SameBrandItemsResponse<CatalogItemDto>> GetCatalogItemsByBrandAsync(int id);
    Task<SameTypeItemsResponse<CatalogItemDto>> GetCatalogItemsByTypeAsync(int id);
    Task<GetBrandsResponse<CatalogBrandDto>> GetCalalogBrandsAsync();
    Task<GetTypesResponse<CatalogTypeDto>> GetCatalogTypesAsync();
}