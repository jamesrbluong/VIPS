using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VIPS.Models;
using VIPS.Models.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddServerSideBlazor();
builder.Services.AddMvc();


builder.Services.AddTransient<IServiceProvider, ServiceProvider>();


builder.Services.AddSession(); 
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


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
            UserName = "admin",
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