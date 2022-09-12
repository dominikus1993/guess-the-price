using GuessThePrice.Core.Grains;
using GuessThePrice.Core.Services;

using Orleans;
using Orleans.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseOrleans(b =>
{
    b.AddMemoryGrainStorage("games");
    b.UseDashboard(options =>
    {
        options.Port = 8888;
    }).ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(GameGrain).Assembly));
});
builder.Services.AddTransient<IProductsDataProvider, ProductsDataProvider>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");;

app.Run();
