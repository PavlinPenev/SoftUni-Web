using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Store_Ge.Data.Models;
using Store_Ge.Data.Repositories;
using Store_Ge.Services.Configurations;
using Store_Ge.Services.Models;
using Store_Ge.Services.Models.ProductModels;
using Store_Ge.Services.Services.AuditTrailService;
using Store_Ge.Services.Services.StoresService;
using System.Linq.Expressions;
using System.Reflection;

namespace Store_Ge.Services.Services.ProductsService
{
    public class ProductsService : IProductsService
    {
        private readonly StoreGeAppSettings appSettings;
        private readonly IDataProtector dataProtector;
        private readonly IRepository<Product> productsRepository;
        private readonly IAuditTrailService auditTrailService;
        private readonly IStoresService storesService;
        private readonly IMapper mapper;

        public ProductsService(
            IDataProtectionProvider dataProtectionProvider, 
            IRepository<Product> productsRepository, 
            IAuditTrailService auditTrailService,
            IStoresService storesService,
            IMapper mapper, 
            IOptions<StoreGeAppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
            this.dataProtector = dataProtectionProvider.CreateProtector(this.appSettings.DataProtectionKey);
            this.productsRepository = productsRepository;
            this.auditTrailService = auditTrailService;
            this.storesService = storesService;
            this.mapper = mapper;
        }

        public async Task<PagedList<ProductDto>> GetStoreProducts(StoreProductsRequestDto request)
        {
            var decryptedStoreId = int.Parse(dataProtector.Unprotect(request.StoreId));

            var products = await productsRepository.GetAll().Where(p => p.StoreId == decryptedStoreId).ToListAsync();

            var mappedProducts = mapper.Map<List<ProductDto>>(products);

            mappedProducts.ForEach(p => p.Id = dataProtector.Protect(p.Id));

            mappedProducts = FilterProducts(mappedProducts, request);

            var pagedProducts = new PagedList<ProductDto>(mappedProducts, products.Count);

            return pagedProducts;
        }

        public async Task<List<ProductDto>> GetStoreAddProducts(string storeId)
        {
            var decryptedStoreId = int.Parse(dataProtector.Unprotect(storeId));

            var products = await productsRepository.GetAll().Where(p => p.StoreId == decryptedStoreId).ToListAsync();

            var mappedProducts = mapper.Map<List<ProductDto>>(products);

            mappedProducts.ForEach(p => p.Id = dataProtector.Protect(p.Id));

            return mappedProducts;
        }

        public async Task<bool> SellProducts(SaleRequestDto request)
        {
            var decryptedStoreId = int.Parse(dataProtector.Unprotect(request.StoreId));
            var store = await storesService.GetStore(request.StoreId);
            if (store == null || !request.Products.Any())
            {
                return false;
            }

            for (int i = 0; i < request.Products.Count; i++)
            {
                if (request.Products[i].Quantity < request.Products[i].PlusQuantity.Value)
                {
                    return false;
                }

                request.Products[i].Id = dataProtector.Unprotect(request.Products[i].Id);
                request.Products[i].Quantity -= request.Products[i].PlusQuantity.Value;
                await auditTrailService.SellProduct(request.Products[i], decryptedStoreId);
            }

            var products = mapper.Map<List<Product>>(request.Products);

            products.ForEach(x => x.StoreId = decryptedStoreId);

            await productsRepository.BulkMerge(products);

            return true;
        }

        public async Task UpsertProducts(List<AddProductDto> addProducts, int storeId)
        {
            for (int i = 0; i < addProducts.Count; i++)
            {
                if (addProducts[i].Id != null)
                {
                    addProducts[i].Id = dataProtector.Unprotect(addProducts[i].Id);
                    await auditTrailService.AddProduct(addProducts[i], storeId);
                }

                if (addProducts[i].PlusQuantity.HasValue)
                {
                    addProducts[i].Quantity += addProducts[i].PlusQuantity.Value;
                    await auditTrailService.AddProductQuantity(addProducts[i], storeId);
                };
            }

            var products = mapper.Map<List<Product>>(addProducts);

            products.ForEach(x => x.StoreId = storeId);

            await productsRepository.BulkMerge(products);
        }

        #region Private Methods
        private List<ProductDto> FilterProducts(List<ProductDto> productsToFilter, StoreProductsRequestDto request)
        {
            Expression<Func<ProductDto, bool>> filterTemplate = x => true;

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                filterTemplate = filterTemplate.And(x => x.Name.Contains(request.SearchTerm));
            }

            productsToFilter = productsToFilter.Where(filterTemplate.Compile()).ToList();

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                var flags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public;

                var orderByPropertyInfo = typeof(ProductDto).GetProperty(request.OrderBy, flags);

                if (!request.IsDescending)
                {
                    productsToFilter = productsToFilter
                        .OrderBy(x => orderByPropertyInfo.GetValue(x)).ToList();
                }
                else
                {
                    productsToFilter = productsToFilter
                        .OrderByDescending(x => orderByPropertyInfo.GetValue(x)).ToList();
                }
            }

            return productsToFilter.Skip(request.Skip).Take(request.Take).ToList();
        }
        #endregion
    }
}
