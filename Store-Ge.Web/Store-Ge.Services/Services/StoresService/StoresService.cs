using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store_Ge.Data.Models;
using Store_Ge.Data.Repositories;
using Store_Ge.Services.Models.StoreModels;
using System.Linq;
using static Store_Ge.Common.Constants.CommonConstants;

namespace Store_Ge.Services.Services.StoresService
{
    public class StoresService : IStoresService
    {
        private readonly IRepository<Store> storeRepository;
        private readonly IRepository<ApplicationUser> userRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly IDataProtector dataProtector;

        public StoresService(
            IRepository<Store> storeRepository,
            IRepository<ApplicationUser> userRepository,
            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            IMapper mapper)
        {
            this.storeRepository = storeRepository;
            this.userRepository = userRepository;
            this.userManager = userManager;
            this.mapper = mapper;
            this.dataProtector = dataProtectionProvider.CreateProtector(STORE_GE_DATA_PROTECTION_STRING_LITERAL);
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
            var decryptedUserId = dataProtector.Unprotect(addStoreDtoRequest.UserId);
            var user = await userManager.FindByIdAsync(addStoreDtoRequest.UserId);

            if (user == null)
            {
                return false;
            }

            var storeToAdd = mapper.Map<Store>(addStoreDtoRequest);
            await storeRepository.AddAsync(storeToAdd);
            await storeRepository.SaveChangesAsync();

            var store = await GetStoreByNameAndUserId(addStoreDtoRequest.UserId, addStoreDtoRequest.Name);

            if (store == null)
            {
                return false;
            }

            var userStore = new UserStore
            {
                UserId = user.Id,
                StoreId = int.Parse(store.Id)
            };

            user.UsersStores.Add(userStore);
            userRepository.Update(user);
            await userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<StoreDto> GetStoreByNameAndUserId(string userId, string name)
        {
            var store = await storeRepository
                .GetAll()
                .FirstOrDefaultAsync(x =>
                    x.Name == name 
                    && x.UsersStores
                        .Select(x => x.UserId)
                        .Contains(int.Parse(userId)));

            var mappedStore = mapper.Map<StoreDto>(store);

            return mappedStore;
        }
    }
}
