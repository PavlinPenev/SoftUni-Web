using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Store_Ge.Data;
using static Store_Ge.Data.Constants.ValidationConstants;
using Store_Ge.Data.Models;
using Store_Ge.Data.Repositories;
using Store_Ge.Services.Services.AccountsService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Store_Ge.Web.Configurations;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);

// Add services to the container.
var dbConnectionStringSection = builder.Configuration.GetSection("DbConfiguration");
builder.Services.Configure<DbConfiguration>(dbConnectionStringSection);

var parameterlessConnectionString = dbConnectionStringSection.Get<DbConfiguration>();
var connectionStringBuilder = new SqlConnectionStringBuilder(parameterlessConnectionString.ConnectionString);
connectionStringBuilder.UserID = builder.Configuration["DbUser"];
connectionStringBuilder.Password = builder.Configuration["DbPassword"];

var connectionString = connectionStringBuilder.ConnectionString;

builder.Services.AddDbContext<StoreGeDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = PASSWORD_MIN_LENGTH;
    options.Lockout.MaxFailedAccessAttempts = MAX_FAILED_LOGIN_ATTEMPTS;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(LOCKOUT_TIMESPAN_IN_MINUTES);
    options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<StoreGeDbContext>();

builder.Services.AddDataProtection();

var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
var jwtKey = Encoding.ASCII.GetBytes(jwtSettings.Secret);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IAccountsService, AccountsService>();

builder.Services.AddSwaggerGen(sg =>
{
    sg.SwaggerDoc("v1", new OpenApiInfo 
        {
            Version = "v1",
            Title = "Store-Ge.Api",
            Description = "The API for the Store-Ge App. Shop storage and product selling manager."
        });
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
