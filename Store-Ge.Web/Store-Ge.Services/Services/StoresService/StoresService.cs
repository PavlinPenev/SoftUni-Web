using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Store_Ge.Data.Models;
using Store_Ge.Data.Repositories;
using Store_Ge.Services.Configurations;
using Store_Ge.Services.Models.StoreModels;
using System.Linq;
using static Store_Ge.Common.Constants.CommonConstants;

namespace Store_Ge.Services.Services.StoresService
{
    public class StoresService : IStoresService
    {
        private readonly IRepository<Store> storeRepository;
        private readonly IRepository<UserStore> userStoreRepository;
        private readonly IRepository<ApplicationUser> userRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly IDataProtector dataProtector;
        private readonly StoreGeAppSettings appSettings;

        public StoresService(
            IRepository<Store> storeRepository,
            IRepository<UserStore> userStoreRepository,
            IRepository<ApplicationUser> userRepository,
            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            IOptions<StoreGeAppSettings> appSettings,
            IMapper mapper)
        {
            this.storeRepository = storeRepository;
            this.userStoreRepository = userStoreRepository;
            this.userRepository = userRepository;
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
    }
}
