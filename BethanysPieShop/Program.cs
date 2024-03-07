using System.Text.Json.Serialization;
using BethanysPieShop.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
                .AddJsonOptions(options=>{
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });


builder.Services.AddDbContext<AppDbContext>(options=>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BethanysPieShop"));
});


builder.Services.AddScoped<IPieRepository, PieRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository,OrderRepository>();

builder.Services.AddScoped<IShoppingCart, ShoppingCart>(sp=> ShoppingCart.GetCart(sp));
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorPages();

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
app.UseSession();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages();


DbInitializer.Seed(app);
app.Run();
