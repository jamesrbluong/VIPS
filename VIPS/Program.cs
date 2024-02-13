using Blazored.Toast;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Common.Data;
using Common.Entities;
using Repositories.Accounts;
using Repositories.Contracts;
using Repositories.Partners;
using Repositories.Schools;
using Services.Contracts;
using Services.Partners;
using Services.Schools;
using Services.Departments;
using Services.Account;
using Services.Visualizations;
using Services.Nodes;
using Services.Edges;
using Repositories.Edges;
using Repositories.Nodes;

// "Server=(localdb)\\MSSQLLocalDB;Database=VIPS;Trusted_Connection=True;MultipleActiveResultSets=true"
// "Server=tcp:vipsserver.database.windows.net,1433;Initial Catalog=vips;Persist Security Info=False;User ID=vipsadmin;Password=VIPS!unf;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddControllersWithViews();
services.AddServerSideBlazor();
services.AddMvc();
//@(await Html.RenderComponentAsync<Visualization>(RenderMode.Server))

services.AddTransient<IServiceProvider, ServiceProvider>();

RegisterRepositories(services);
RegisterServices(services);

services.AddSession();
services.AddHttpContextAccessor();
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("VIPS")));
services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
{
    // Lockout settings
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/Login";
});

services.AddAuthentication().AddCookie();

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

void RegisterRepositories(IServiceCollection services)
{
    services.AddScoped<IContractRepository, ContractRepository>();
    services.AddScoped<IAccountRepository, AccountRepository>();
    services.AddScoped<ISchoolRepository, SchoolRepository>();
    services.AddScoped<IDepartmentRepository, DepartmentRepository>();
    services.AddScoped<IPartnerRepository, PartnerRepository>();
    services.AddScoped<IEdgeRepository, EdgeRepository>();
    services.AddScoped<INodeRepository, NodeRepository>();
}

void RegisterServices(IServiceCollection services)
{
    services.AddScoped<IContractService, ContractService>();
    services.AddScoped<IAccountService, AccountService>();
    services.AddScoped<ISchoolService, SchoolService>();
    services.AddScoped<IDepartmentService, DepartmentService>();
    services.AddScoped<IPartnerService, PartnerService>();
    services.AddScoped<IVisualizationService, VisualizationService>();
    services.AddScoped<IEdgeService, EdgeService>();
    services.AddScoped<INodeService, NodeService>();
}

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