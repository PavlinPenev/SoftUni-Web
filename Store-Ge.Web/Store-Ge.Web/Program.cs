using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Store_Ge.Data;
using static Store_Ge.Data.Constants.ValidationConstants;
using Store_Ge.Data.Models;
using Store_Ge.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionStringBuilder = new SqlConnectionStringBuilder(
    builder.Configuration[builder.Configuration.GetConnectionString("DefaultConnection")]);
connectionStringBuilder.UserID = builder.Configuration["DbUser"];
connectionStringBuilder.Password = builder.Configuration["DbPassword"];

var connectionString = connectionStringBuilder.ConnectionString;

builder.Services.AddDbContext<StoreGeDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = PASSWORD_MIN_LENGTH;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
}).AddEntityFrameworkStores<StoreGeDbContext>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddSwaggerGen(sg =>
{
    sg.SwaggerDoc("v1", new OpenApiInfo 
        {
            Version = "v1",
            Title = "Store-Ge.Api",
            Description = "The API for the Store-Ge App. Your shop storage and product selling manager."
        });
});

builder.Services.AddControllers();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<StoreGeDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
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
