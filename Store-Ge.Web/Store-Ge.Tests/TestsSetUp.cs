using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Store_Ge.Configurations.Services;
using Store_Ge.Data;
using Store_Ge.Data.Models;
using Store_Ge.Data.Repositories;
using Store_Ge.Services.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store_Ge.Tests
{
    public class TestsSetUp
    {
        protected const string MOCK_USER_ID = "mockUserId";
        protected const string MOCK_REFRESH_TOKEN = "mockRefreshToken";
        protected const string MOCK_STORE_ID = "mockStoreId";
        protected const string MOCK_EMAIL_ADDRESS = "asfg@asg.bas";
        protected const string MOCK_EMAIL_ADDRESS_FOR_UPDATE = "mockEmailUpdated@asd.bg";
        protected const string MOCK_USERNAME = "mockPaf";

        protected List<ApplicationUser> users;
        protected List<ApplicationRole> roles;
        protected IQueryable<AuditEvent> auditEvents;
        protected StoreGeDbContext context;

        public void InitializeDbContext()
        {
            if (context != null)
            {
                context.Database.EnsureDeleted();
            }
            var options = new DbContextOptionsBuilder<StoreGeDbContext>()
                .UseInMemoryDatabase("StoreGeInMemoryDB").Options;

            context = new StoreGeDbContext(options);
        }

        public IRepository<ApplicationUser> GetUserRepository()
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            users = new List<ApplicationUser> 
            {
                new ApplicationUser
                {
                    Id = 1,
                    UserName = "Paf",
                    Email = "asfg@asg.bas",
                    EmailConfirmed = true,
                    CreatedOn = DateTime.UtcNow,
                    NormalizedEmail = "ASFG@ASG.BAS",
                    NormalizedUserName = "PAF",
                    RefreshToken = "mockRefreshToken",
                    RefreshTokenExpirationDate = DateTime.UtcNow.AddHours(1)  
                },
                new ApplicationUser
                {
                    Id = 2,
                    UserName = "Viki",
                    Email = "bsfg@asg.bas",
                    EmailConfirmed = true,
                    CreatedOn = DateTime.UtcNow,
                    NormalizedEmail = "BSFG@ASG.BAS",
                    NormalizedUserName = "VIKI",
                    RefreshToken = "mockRefreshToken",
                    RefreshTokenExpirationDate = DateTime.UtcNow.AddHours(1)
                },
                new ApplicationUser
                {
                    Id = 3,
                    UserName = "Rosi",
                    Email = "csfg@asg.bas",
                    EmailConfirmed = false,
                    CreatedOn = DateTime.UtcNow,
                    NormalizedEmail = "CSFG@ASG.BAS",
                    NormalizedUserName = "ROSI",
                    RefreshToken = "mockRefreshToken",
                    RefreshTokenExpirationDate = DateTime.UtcNow.AddHours(1)
                }
            };

            roles = new List<ApplicationRole>
                {
                    new ApplicationRole
                    {
                        Id = 1,
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    },
                    new ApplicationRole
                    {
                        Id = 2,
                        Name = "Cashier",
                        NormalizedName = "CASHIER"
                    }
                };

            foreach (var user in users)
            {
                user.PasswordHash = passwordHasher.HashPassword(user, "Aa!123456");
            }

            var mockRepo = new Mock<IRepository<ApplicationUser>>();

            mockRepo.Setup(x => x.GetAll()).Returns(users.AsQueryable());
                

            return mockRepo.Object;
        }

        public IRepository<AuditEvent> GetAuditTrailRepository()
        {
            var dbSet = context.Set<AuditEvent>();
            dbSet.AddRange(new List<AuditEvent>
            {
                new AuditEvent
                {
                    Id = 1,
                    Action = "MockAction",
                    Description = "A mock action for test purposes",
                    CreatedOn = DateTime.UtcNow,
                    StoreId = 6
                },
                new AuditEvent
                {
                    Id = 2,
                    Action = "MockActionSecond",
                    Description = "A second mock action for test purposes",
                    CreatedOn = DateTime.UtcNow,
                    StoreId = 2
                }
            });
            context.SaveChangesAsync();

            var mockRepo = new Mock<Repository<AuditEvent>>(context);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<AuditEvent>())).Callback<AuditEvent>(x => context.AddAsync(x));

            return mockRepo.Object;
        }

        public UserManager<ApplicationUser> GetUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            var mgr = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

            mgr.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<ApplicationUser, string>((x, y) => users.Add(x));
            mgr.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            mgr.Setup(x => x.IsEmailConfirmedAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(true);
            mgr.Setup(x => x.FindByEmailAsync(MOCK_EMAIL_ADDRESS))
                .ReturnsAsync(users.FirstOrDefault(y => y.Email == MOCK_EMAIL_ADDRESS));
            mgr.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(roles.Select(x => x.Name).ToList());
            mgr.Setup(x => x.AddToRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<List<string>>()))
                .ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync("mockToken");
            mgr.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(users[0]);
            mgr.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.SetEmailAsync(users[0], "mockEmail@asd.bg"))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<ApplicationUser, string>((x, y) => x.Email = y);
            mgr.Setup(x => x.SetEmailAsync(users[0], MOCK_EMAIL_ADDRESS_FOR_UPDATE))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<ApplicationUser, string>((x, y) => x.Email = MOCK_EMAIL_ADDRESS_FOR_UPDATE);
            mgr.Setup(x => x.SetUserNameAsync(users[0], "mockPaf"))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<ApplicationUser, string>((x, y) => x.UserName = y);

            return mgr.Object;
        }

        public RoleManager<ApplicationRole> GetRoleManager()
        {
            var store = new Mock<IRoleStore<ApplicationRole>>();
            var mgr = new Mock<RoleManager<ApplicationRole>>(
                store.Object, null, null, null, null);

            mgr.Setup(x => x.Roles).Returns(roles.AsQueryable());
            mgr.Setup(x => x.FindByIdAsync("2")).ReturnsAsync(roles.Where(r => r.Id.ToString() == "2").FirstOrDefault());

            return mgr.Object;
        }

        public static IDataProtectionProvider GetProtectionProvider()
        {
            Mock<IDataProtector> mockDataProtector = new Mock<IDataProtector>();
            mockDataProtector.Setup(sut => sut.Protect(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes("protectedText"));
            mockDataProtector.Setup(sut => sut.Unprotect(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes("6"));

            Mock<IDataProtectionProvider> mockDataProtectionProvider = new Mock<IDataProtectionProvider>();

            mockDataProtectionProvider.Setup(s => s.CreateProtector(It.IsAny<string>())).Returns(mockDataProtector.Object);

            return mockDataProtectionProvider.Object;
        }

        public static IOptions<JwtSettings> GetJwtSettingsOptions()
        {
            var config = GetIConfiguration();

            var jwtSettings = new JwtSettings
            {
                Secret = config.GetSection("JwtSettings").GetValue<string>("Secret")
            };

            var jwtOptions = Options.Create(jwtSettings);

            return jwtOptions;
        }


        public static IOptions<StoreGeAppSettings> GetStoreGeAppSettings()
        {
            var config = GetIConfiguration();

            var storeGeAppSettingsSection = config.GetSection("StoreGeAppSettings");

            var storeGeAppSettings = new StoreGeAppSettings
            {
                StoreGeAppBaseUrl = storeGeAppSettingsSection.GetValue<string>("StoreGeAppBaseUrl"),
                StoreGeAppConfirmEmailUrl = storeGeAppSettingsSection.GetValue<string>("StoreGeAppConfirmEmailUrl"),
                StoreGeAppResetPasswordUrl = storeGeAppSettingsSection.GetValue<string>("StoreGeAppResetPasswordUrl"),
                StoreGeAppLoginUrl = storeGeAppSettingsSection.GetValue<string>("StoreGeAppLoginUrl"),
                DataProtectionKey = storeGeAppSettingsSection.GetValue<string>("DataProtectionKey")
            };

            var storeGeAppSettingsOptions = Options.Create(storeGeAppSettings);

            return storeGeAppSettingsOptions;
        }

        public static IMapper GetAutoMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies())
            );

            var mapper = new Mapper(configuration);

            return mapper;
        }

        private static IConfigurationRoot GetIConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
