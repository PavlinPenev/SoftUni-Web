using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Store_Ge.Data.Models;
using Store_Ge.Data.Repositories;
using Store_Ge.Services.Configurations;
using Store_Ge.Services.Models;
using Store_Ge.Services.Models.SupplierModels;
using Store_Ge.Services.Services.AccountsService;
using Store_Ge.Services.Services.AuditTrailService;
using System.Linq.Expressions;
using System.Reflection;

namespace Store_Ge.Services.Services.SuppliersService
{
    public class SuppliersService : ISuppliersService
    {
        private readonly IAccountsService accountsService;
        private readonly IAuditTrailService auditTrailService;
        private readonly IRepository<StoreSupplier> storeSupplierRepository;
        private readonly IRepository<UserStore> userStoreRepository;
        private readonly IRepository<Supplier> supplierRepository;
        private readonly IMapper mapper;
        private readonly IDataProtector dataProtector;
        private readonly StoreGeAppSettings appSettings;


        public SuppliersService(
            IAccountsService accountsService,
            IAuditTrailService auditTrailService,
            IDataProtectionProvider dataProtectionProvider, 
            IOptions<StoreGeAppSettings> appSettingsOptions, 
            IRepository<StoreSupplier> storeSupplierRepository,
            IRepository<UserStore> userStoreRepository,
            IRepository<Supplier> supplierRepository,
            IMapper mapper)
        {
            this.accountsService = accountsService;
            this.auditTrailService = auditTrailService;
            this.storeSupplierRepository = storeSupplierRepository;
            this.userStoreRepository = userStoreRepository;
            this.supplierRepository = supplierRepository;
            this.mapper = mapper;
            this.appSettings = appSettingsOptions.Value;
            this.dataProtector = dataProtectionProvider.CreateProtector(appSettings.DataProtectionKey);
        }

        public async Task<List<AddOrderSupplierDto>> GetUserSuppliers(string userId)
        {
            var suppliersDto = new List<AddOrderSupplierDto>();

            var user = await accountsService.GetUser(userId);
            if (user == null)
            {
                return suppliersDto;
            }

            var decryptedUserId = dataProtector.Unprotect(userId);

            var storeIds = await userStoreRepository
                .GetAll()
                .Where(x => x.UserId == int.Parse(decryptedUserId))
                .Select(x => x.StoreId)
                .ToListAsync();

            var supplierIds = await storeSupplierRepository
                .GetAll()
                .Where(x => storeIds.Contains(x.StoreId))
                .Select(x => x.SupplierId)
                .ToListAsync();

            var suppliers = await supplierRepository
                .GetAll()
                .Where(x => supplierIds.Contains(x.Id))
                .ToListAsync();

            suppliersDto = mapper.Map<List<AddOrderSupplierDto>>(suppliers);

            suppliersDto.ForEach(x => x.Id = dataProtector.Protect(x.Id));

            return suppliersDto;
        }

        public async Task<PagedList<UserSupplierDto>> GetUserSuppliersPaged(UserSuppliersRequestDto request)
        {
            var suppliersDto = new List<UserSupplierDto>();

            var user = await accountsService.GetUser(request.UserId);
            if (user == null)
            {
                return new PagedList<UserSupplierDto>
                {
                    Items = suppliersDto,
                    TotalItemsCount = 0
                };
            }

            var decryptedUserId = dataProtector.Unprotect(request.UserId);

            var storeIds = await userStoreRepository
                .GetAll()
                .Where(x => x.UserId == int.Parse(decryptedUserId))
                .Select(x => x.StoreId)
                .ToListAsync();

            var supplierIds = await storeSupplierRepository
                .GetAll()
                .Where(x => storeIds.Contains(x.StoreId))
                .Select(x => x.SupplierId)
                .ToListAsync();

            var suppliers = await supplierRepository
                .GetAll()
                .Where(x => supplierIds.Contains(x.Id))
                .ToListAsync();

            suppliersDto = mapper.Map<List<UserSupplierDto>>(suppliers);

            suppliersDto = FilterSuppliers(suppliersDto, request);

            suppliersDto.ForEach(x => dataProtector.Protect(x.Id));

            return new PagedList<UserSupplierDto>
            {
                Items = suppliersDto,
                TotalItemsCount = suppliers.Count
            }; ;
        }

        public async Task<bool> AddSupplier(AddSupplierRequestDto request)
        {
            var user = await accountsService.GetUser(request.UserId);
            if (user == null)
            {
                return false;
            }

            var decryptedUserId = int.Parse(dataProtector.Unprotect(request.UserId));

            var supplier = new Supplier
            {
                Name = request.Name
            };

            var storeIds = await userStoreRepository
                .GetAll()
                .Where(x => x.UserId == decryptedUserId)
                .Select(x => x.StoreId)
                .ToListAsync();

            foreach (var id in storeIds)
            {
                supplier.StoresSuppliers.Add(new StoreSupplier
                {
                    StoreId = id,
                    Supplier = supplier
                });
            }

            await supplierRepository.AddAsync(supplier);
            var result = await supplierRepository.SaveChangesAsync();

            if (result == 0)
            {
                return false;
            }

            await auditTrailService.AddSupplier(supplier, storeIds);

            return true;
        }

        #region Private Methods

        private List<UserSupplierDto> FilterSuppliers(List<UserSupplierDto> suppliers, UserSuppliersRequestDto request)
        {
            Expression<Func<UserSupplierDto, bool>> filterTemplate = x => true;

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                filterTemplate = filterTemplate.And(x => x.Name.Contains(request.SearchTerm));
            }

            if (request.DateAddedFrom.HasValue || request.DateAddedTo.HasValue)
            {
                if (request.DateAddedFrom.HasValue && !request.DateAddedTo.HasValue)
                {
                    filterTemplate = filterTemplate.And(x => x.CreatedOn >= request.DateAddedFrom);
                }
                else if (!request.DateAddedFrom.HasValue && request.DateAddedTo.HasValue)
                {
                    filterTemplate = filterTemplate.And(x => x.CreatedOn <= request.DateAddedTo);
                }
                else
                {
                    filterTemplate = filterTemplate.And(x =>
                        x.CreatedOn >= request.DateAddedFrom && x.CreatedOn <= request.DateAddedTo);
                }
            }

            suppliers = suppliers.Where(filterTemplate.Compile()).ToList();

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                var flags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public;

                var orderByPropertyInfo = typeof(UserSupplierDto).GetProperty(request.OrderBy, flags);

                if (!request.IsDescending)
                {
                    suppliers = suppliers
                        .OrderBy(x => orderByPropertyInfo.GetValue(x)).ToList();
                }
                else
                {
                    suppliers = suppliers
                        .OrderByDescending(x => orderByPropertyInfo.GetValue(x)).ToList();
                }
            }

            return suppliers.Skip(request.Skip).Take(request.Take).ToList();
        }

        #endregion
    }
}
