using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Store_Ge.Data.Models;
using Store_Ge.Data.Repositories;
using Store_Ge.Services.Configurations;
using Store_Ge.Services.Models.SupplierModels;
using Store_Ge.Services.Services.AccountsService;

namespace Store_Ge.Services.Services.SuppliersService
{
    public class SuppliersService : ISuppliersService
    {
        private readonly IAccountsService accountsService;
        private readonly IRepository<StoreSupplier> storeSupplierRepository;
        private readonly IRepository<UserStore> userStoreRepository;
        private readonly IRepository<Supplier> supplierRepository;
        private readonly IMapper mapper;
        private readonly IDataProtector dataProtector;
        private readonly StoreGeAppSettings appSettings;


        public SuppliersService(
            IAccountsService accountsService,
            IDataProtectionProvider dataProtectionProvider, 
            IOptions<StoreGeAppSettings> appSettingsOptions, 
            IRepository<StoreSupplier> storeSupplierRepository,
            IRepository<UserStore> userStoreRepository,
            IRepository<Supplier> supplierRepository,
            IMapper mapper)
        {
            this.accountsService = accountsService;
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

            return suppliersDto;
        }
    }
}
