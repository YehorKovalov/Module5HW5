using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Response.BrandResponses;
using Catalog.Host.Models.Response.ItemResponses;
using Catalog.Host.Models.Response.TypeResponses;
using Catalog.Host.Repositories;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogService : BaseDataService<ApplicationDbContext>, ICatalogService
{
    private readonly ICatalogItemRepository _catalogItemRepository;
    private readonly ICatalogBrandRepository _catalogBrandRepository;
    private readonly ICatalogTypeRepository _catalogTypeRepository;
    private readonly IMapper _mapper;

    public CatalogService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository,
        ICatalogBrandRepository catalogBrandRepository,
        ICatalogTypeRepository catalogTypeRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _catalogItemRepository = catalogItemRepository;
        _catalogBrandRepository = catalogBrandRepository;
        _catalogTypeRepository = catalogTypeRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedItemsResponse<CatalogItemDto>?> GetCatalogItemsAsync(int pageSize, int pageIndex, Dictionary<CatalogTypeFilter, int>? filters)
    {
        return await ExecuteSafeAsync(async () =>
        {
            int? brandFilter = null;
            int? typeFilter = null;

            if (filters != null)
            {
                if (filters.TryGetValue(CatalogTypeFilter.Brand, out var brand))
                {
                    brandFilter = brand;
                }

                if (filters.TryGetValue(CatalogTypeFilter.Type, out var type))
                {
                    typeFilter = type;
                }
            }

            var result = await _catalogItemRepository.GetByPageAsync(pageIndex, pageSize, brandFilter, typeFilter);
            if (result == null)
            {
                return null;
            }

            return new PaginatedItemsResponse<CatalogItemDto>()
            {
                Count = result.TotalCount,
                Data = result.Data.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        });
    }

    public async Task<GetItemByIdResponse<CatalogItemDto>> GetCatalogItemByIdAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogItemRepository.GetCatalogItemByIdAsync(id);
            return new GetItemByIdResponse<CatalogItemDto>
            {
                Item = _mapper.Map<CatalogItemDto>(result)
            };
        });
    }

    public async Task<SameBrandItemsResponse<CatalogItemDto>> GetCatalogItemsByBrandAsync(int brandId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogItemRepository.GetCatalogItemsByBrandAsync(brandId);
            return new SameBrandItemsResponse<CatalogItemDto>()
            {
                Data = result.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
            };
        });
    }

    public async Task<SameTypeItemsResponse<CatalogItemDto>> GetCatalogItemsByTypeAsync(int typeId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogItemRepository.GetCatalogItemsByTypeAsync(typeId);
            return new SameTypeItemsResponse<CatalogItemDto>()
            {
                Data = result.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
            };
        });
    }

    public async Task<GetBrandsResponse<CatalogBrandDto>> GetCalalogBrandsAsync()
{
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogBrandRepository.GetBrandesAsync();
            return new GetBrandsResponse<CatalogBrandDto>
            {
                Data = result.Select(b => _mapper.Map<CatalogBrandDto>(result)).ToList()
            };
        });
    }

    public async Task<GetTypesResponse<CatalogTypeDto>> GetCatalogTypesAsync()
{
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogTypeRepository.GetTypesAsync();
            return new GetTypesResponse<CatalogTypeDto>
            {
                Data = result.Select(b => _mapper.Map<CatalogTypeDto>(result)).ToList()
            };
        });
    }
}