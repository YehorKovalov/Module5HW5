using Catalog.Host.Data;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogItemRepository
{
    Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, int? brandFilter, int? typeFilter);
    Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
    Task<int?> Update(CatalogItem item);
    Task<int?> Remove(int itemId);
    Task<CatalogItem?> GetCatalogItemByIdAsync(int id);
    Task<IEnumerable<CatalogItem>> GetCatalogItemsByBrandAsync(int brandId);
    Task<IEnumerable<CatalogItem>> GetCatalogItemsByTypeAsync(int typeId);
}