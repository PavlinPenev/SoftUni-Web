using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using Store_Ge.Data.Models;
using Store_Ge.Data.Repositories;
using Store_Ge.Services.Configurations;
using Store_Ge.Services.Models.StoreModels;
using Store_Ge.Services.Services.AuditTrailService;

namespace Store_Ge.Services.Services.StoresService
{
    public class StoresService : IStoresService
    {
        private readonly IRepository<Store> storeRepository;
        private readonly IAuditTrailService auditTrailService;
        private readonly IRepository<UserStore> userStoreRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly IDataProtector dataProtector;
        private readonly StoreGeAppSettings appSettings;

        public StoresService(
            IRepository<Store> storeRepository,
            IAuditTrailService auditTrailService,
            IRepository<UserStore> userStoreRepository,
            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            IOptions<StoreGeAppSettings> appSettings,
            IMapper mapper)
        {
            this.storeRepository = storeRepository;
            this.auditTrailService = auditTrailService;
            this.userStoreRepository = userStoreRepository;
            this.userManager = userManager;
            this.mapper = mapper;
            this.appSettings = appSettings.Value;
            this.dataProtector = dataProtectionProvider.CreateProtector(this.appSettings.DataProtectionKey);
        }

        public async Task<List<StoreDto>> GetStores(string userId)
        {
            var decryptedUserId = int.Parse(dataProtector.Unprotect(userId));

            var stores = await storeRepository
                .GetAll()
                .Where(x => x.UsersStores
                    .Select(y => y.UserId)
                    .Contains(decryptedUserId))
                .ToListAsync();

            var mappedStores = mapper.Map<List<StoreDto>>(stores);

            foreach (var store in mappedStores)
            {
                store.Id = dataProtector.Protect(store.Id.ToString());
            }

            return mappedStores;
        }

        public async Task<bool> AddStore(AddStoreDto addStoreDtoRequest)
        {
            addStoreDtoRequest.UserId = dataProtector.Unprotect(addStoreDtoRequest.UserId);
            var user = await userManager.FindByIdAsync(addStoreDtoRequest.UserId);

            if (user == null)
            {
                return false;
            }

            var storeToAdd = mapper.Map<Store>(addStoreDtoRequest);
            storeToAdd.UsersStores.Add(new UserStore
            {
                User = user,
                StoreId = storeToAdd.Id
            });

            await storeRepository.AddAsync(storeToAdd);
            await storeRepository.SaveChangesAsync();

            return true;
        }

        public async Task<StoreDto> GetStore(string storeId)
        {
            var decryptedStoreId = dataProtector.Unprotect(storeId);

            var store = await storeRepository.GetAll().FirstOrDefaultAsync(x => x.Id == int.Parse(decryptedStoreId));

            var mappedStore = mapper.Map<StoreDto>(store);

            mappedStore.Id = dataProtector.Protect(mappedStore.Id);

            return mappedStore;
        }

        public async Task<StoreDto> GetStoreByNameAndUserId(string userId, string name)
        {
            var userStoresIds = await userStoreRepository.GetAll().Where(x => x.UserId == int.Parse(userId)).Select(x => x.StoreId).ToListAsync();

            var store = await storeRepository
                .GetAll()
                .SingleOrDefaultAsync(x =>
                    x.Name == name 
                    && userStoresIds
                        .Contains(x.Id));

            var mappedStore = mapper.Map<StoreDto>(store);

            return mappedStore;
        }

        public async Task<byte[]> GetReportFile(string storeId)
        {
            var decodedStoreId = int.Parse(dataProtector.Unprotect(storeId));

            var auditEvents = await auditTrailService.GetAll(decodedStoreId);

            if (auditEvents == null)
            {
                return null;
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Event Report");

                //header row
                worksheet.Cells["A1"].Value = "Action";
                worksheet.Cells["A1"].AutoFitColumns();
                worksheet.Cells["B1"].Value = "Description";
                worksheet.Cells["B1"].AutoFitColumns();
                worksheet.Cells["C1"].Value = "Created On";
                worksheet.Cells["C1"].AutoFitColumns();

                //data rows
                for (int i = 1; i <= auditEvents.Count; i++)
                {
                    worksheet.Cells[i + 1, 1].Value = auditEvents[i - 1].Action;
                    worksheet.Cells[i + 1, 1].AutoFitColumns();
                    worksheet.Cells[i + 1, 2].Value = auditEvents[i - 1].Description;
                    worksheet.Cells[i + 1, 2].AutoFitColumns();
                    worksheet.Cells[i + 1, 3].Value = auditEvents[i - 1].CreatedOn.ToString("MM/dd/yyyy");
                    worksheet.Cells[i + 1, 3].AutoFitColumns();
                }

                var excelData = package.GetAsByteArray();

                return excelData;
            }
        }
    }
}
