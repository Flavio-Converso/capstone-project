using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Interfaces.Auth;
using capstone_project.Services;
using capstone_project.Services.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

//CONNECTION -> DATACONTEXT
var conn = builder.Configuration.GetConnectionString("DB");
builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(conn));


////AUTH
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";  //todo
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });


//POLICIES
builder.Services.AddAuthorization(options =>
{
    // Master Policy
    options.AddPolicy("MasterPolicy", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "master"); // [Authorize(Policy = "MasterPolicy")]
    });
});


// Add services to the container.
builder.Services.AddControllersWithViews();

// Services
builder.Services
    .AddScoped<ICategoryService, CategoryService>()
    .AddScoped<IPegiService, PegiService>()
    .AddScoped<IRestrictionService, RestrictionService>()
    .AddScoped<IGameService, GameService>()
    .AddScoped<IWishlistService, WishlistService>()
    .AddScoped<ICartService, CartService>()
    .AddScoped<IAuthService, AuthService>()
    .AddScoped<IPasswordHelper, PasswordHelper>();

var app = builder.Build();

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
