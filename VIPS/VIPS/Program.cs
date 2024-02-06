<<<<<<< Updated upstream
using Blazored.Toast;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VIPS.Models;
using VIPS.Models.Data;
=======
using Common.Data;
using Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories.Accounts;
using Repositories.Contracts;
using Repositories.Partners;
using Repositories.Schools;
using Services.Contracts;
>>>>>>> Stashed changes

// "Server=(localdb)\\MSSQLLocalDB;Database=VIPS;Trusted_Connection=True;MultipleActiveResultSets=true"
// "Server=tcp:vipsserver.database.windows.net,1433;Initial Catalog=vips;Persist Security Info=False;User ID=vipsadmin;Password=VIPS!unf;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddServerSideBlazor();
builder.Services.AddMvc();
//@(await Html.RenderComponentAsync<Visualization>(RenderMode.Server))

builder.Services.AddTransient<IServiceProvider, ServiceProvider>();


builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
    {
        // Lockout settings
        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
    }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";  
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/Login";
});

builder.Services.AddAuthentication().AddCookie();

var app = builder.Build();

// create roles service
var service = builder.Services.BuildServiceProvider();

if (service != null)
{
    await CreateRoles(service);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession(); // ADDED

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapBlazorHub(); 


app.Run();

<<<<<<< Updated upstream
=======
void RegisterRepositories(IServiceCollection services)
{
    services.AddScoped<IContractRepository, ContractRepository>();
    services.AddScoped<IAccountRepository, AccountRepository>();
    services.AddScoped<ISchoolRepository, SchoolRepository>();
    services.AddScoped<IDepartmentRepository, DepartmentRepository>();
    services.AddScoped<IPartnerRepository, PartnerRepository>();
    services.AddScoped<IContractRepository, ContractRepository>();
}

void RegisterServices(IServiceCollection services)
{
    services.AddScoped<IPartnerService, ContractService>();
}

>>>>>>> Stashed changes
Console.WriteLine("test gitignore 2");
async Task CreateRoles(IServiceProvider serviceProvider)
{
    Console.WriteLine("Initialize admin and roles");
    // Intitialize Role Manager and User Manager
    RoleManager<IdentityRole<Guid>> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    UserManager<AppUser> userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
    
    IdentityResult roleResult; // Result of identity operation

    string[] roleNames = { "Admin", "CCBL_Employee", "UNF_Employee" }; // string array of all the roles in this program
    
    // For each role in array, create the role if it does not yet exist in the program
    foreach (var name in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(name);
        // ensure that the role does not exist
        if (!roleExist)
        {
            roleResult = await roleManager.CreateAsync(new IdentityRole<Guid>(name));
        }
    }

    // initialize _adminUser with the user with the admin email 
    var _adminUser = await userManager.FindByEmailAsync("admin@unf.edu");

    // check if the user exists
    if (_adminUser == null)
    {
        // create adminUser if they do not already exist
        var adminUser = new AppUser
        {
            UserName = "admin@unf.edu",
            Email = "admin@unf.edu",
        };
        string adminPassword = "Admin123*";

        var createPowerUser = await userManager.CreateAsync(adminUser, adminPassword);
        if (createPowerUser.Succeeded)
        {
            // assign the new user to the admin role
            await userManager.AddToRoleAsync(adminUser, "Admin");

        }
    }
}