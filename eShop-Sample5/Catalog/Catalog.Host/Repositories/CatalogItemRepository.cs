using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;

namespace Catalog.Host.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, int? brandFilter, int? typeFilter)
    {
        IQueryable<CatalogItem> query = _dbContext.CatalogItems;

        if (brandFilter.HasValue)
        {
            query = query.Where(w => w.CatalogBrandId == brandFilter.Value);
        }

        if (typeFilter.HasValue)
        {
            query = query.Where(w => w.CatalogTypeId == typeFilter.Value);
        }

        var totalItems = await query.LongCountAsync();

        var itemsOnPage = await query.OrderBy(c => c.Name)
           .Include(i => i.CatalogBrand)
           .Include(i => i.CatalogType)
           .Skip(pageSize * pageIndex)
           .Take(pageSize)
           .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var item1 = new CatalogItem
        {
            CatalogBrandId = catalogBrandId,
            CatalogTypeId = catalogTypeId,
            Description = description,
            Name = name,
            PictureFileName = pictureFileName,
            Price = price
        };
        var item = await _dbContext.AddAsync(item1);

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<CatalogItem?> GetCatalogItemByIdAsync(int id)
    {
        var item = await _dbContext.CatalogItems.FirstOrDefaultAsync(i => i.Id == id);
        if (item == null)
        {
            return null;
        }

        _dbContext.Entry(item).Reference(i => i.CatalogType).Load();
        _dbContext.Entry(item).Reference(i => i.CatalogBrand).Load();
        return item;
    }

    public async Task<IEnumerable<CatalogItem>> GetCatalogItemsByBrandAsync(int brandId)
    {
        return await _dbContext.CatalogItems
            .Where(i => i.CatalogBrandId == brandId)
            .OrderBy(i => i.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<CatalogItem>> GetCatalogItemsByTypeAsync(int typeId)
    {
        return await _dbContext.CatalogItems
            .Where(i => i.CatalogTypeId == typeId)
            .OrderBy(i => i.Name)
            .ToListAsync();
    }

    public async Task<int?> Update(CatalogItem item)
    {
        if (item == null)
        {
            return null;
        }

        var result = _dbContext.Update(item);
        await _dbContext.SaveChangesAsync();
        return result.Entity.Id;
    }

    public async Task<int?> Remove(int itemId)
    {
        var item = await _dbContext.CatalogItems.FirstOrDefaultAsync(c => c.Id == itemId);

        if (item == null)
        {
            return null;
        }

        var result = _dbContext.CatalogItems.Remove(item);
        await _dbContext.SaveChangesAsync();
        return result.Entity.Id;
    }
}