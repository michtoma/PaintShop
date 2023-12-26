using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PaintShopMVC.Data;
using PaintShopMVC.Interfaces;
using PaintShopMVC.Repository;
using Stripe;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MyDatabaseConnection");
var clientId = builder.Configuration["Authentication:Google:ClientId"];
var clientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
builder.Services.AddDbContext<AppdbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddIdentity<AppUsers, IdentityRole>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppdbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = clientId;
    options.ClientSecret = clientSecret;
});
builder.Services.AddRazorPages();
builder.Services.AddScoped<UserManager<AppUsers>>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShopingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddAuthorization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pl-PL");
});
var stripeSection = builder.Configuration.GetSection("Stripe");
StripeConfiguration.ApiKey = stripeSection["SecretKey"];
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
app.MapRazorPages();
app.Run();
