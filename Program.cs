using Microsoft.AspNetCore.Identity;
using Diploma.Services.Register;
using Diploma.Data;
using Microsoft.EntityFrameworkCore;
using Diploma.Entities;
using Diploma.Services;
using Diploma.Data.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WhiteServiceConnectionString")));

builder.Services.AddIdentity<RegisteredUser, IdentityRole>(options => {
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 1;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

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

try
{
    await DatabaseInitializer.Initialize(app);

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        
        string adminId = await UserSeed.Seed(app);
        ProductSeed.Initialize(services, adminId);
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while seeding the database.");
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
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