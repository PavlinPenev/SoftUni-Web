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
using System.Linq.Expressions;
using System.Reflection;

namespace Store_Ge.Services.Services.ProductsService
{
    public class ProductsService : IProductsService
    {
        private readonly StoreGeAppSettings appSettings;
        private readonly IDataProtector dataProtector;
        private readonly IRepository<Product> productsRepository;
        private readonly IMapper mapper;

        public ProductsService(
            IDataProtectionProvider dataProtectionProvider, 
            IRepository<Product> productsRepository, 
            IMapper mapper, 
            IOptions<StoreGeAppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
            this.dataProtector = dataProtectionProvider.CreateProtector(this.appSettings.DataProtectionKey);
            this.productsRepository = productsRepository;
            this.mapper = mapper;
        }

        public async Task<PagedList<ProductDto>> GetStoreProducts(StoreProductsRequestDto request)
        {
            var decryptedStoreId = int.Parse(dataProtector.Unprotect(request.StoreId));

            var products = await productsRepository.GetAll().Where(p => p.StoreId == decryptedStoreId).ToListAsync();

            var mappedProducts = mapper.Map<List<ProductDto>>(products);

            mappedProducts.ForEach(p => p.Id = dataProtector.Protect(p.Id));

            mappedProducts = FilterProducts(mappedProducts, request);

            var pagedProducts = new PagedList<ProductDto>(mappedProducts);

            return pagedProducts;
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
