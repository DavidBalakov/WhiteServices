using Diploma.Data;
using Diploma.Data.Seed;
using Diploma.Entities;
using Diploma.Services;
using Diploma.Services.Register;
using JewelryShop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("WhiteServiceConnectionString")));

builder.Services.AddDefaultIdentity<RegisteredUser>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 1;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "LoginCookie";
    options.LoginPath = "/RegisteredUser/LogIn";
    options.AccessDeniedPath = "/RegisteredUser/LogIn";
});

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IRegisteredUserService, RegisteredUserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ILogisterService, LogisterService>();

var app = builder.Build();

await DatabaseInitializer.Initialize(app);

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    string adminId = await UserSeed.Seed(app);
    ProductSeed.Initialize(services, adminId);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();