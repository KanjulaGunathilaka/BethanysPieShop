using BethanysPieShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;

var builder = WebApplication.CreateBuilder(args);

//Services
builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
builder.Services.AddScoped<IPieRepository,PieRepository>();

//framework services that enable MVC in the application
builder.Services.AddControllersWithViews();

//add framework services using, an extension method, DbContext extension method
//It has as a type parameter BethanysPieShopDbcontext, so, register that one to be used as the DbContext for the application
builder.Services.AddDbContext<BethanysPieShopDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:BethanysPieShopDbContextConnection"]);
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();

app.MapControllerRoute(name:"default", pattern:"{controller=Home}/{action=Index}/{id:int?}");

//Call the DbInitializer
DbInitializer.Seed(app);

app.Run();
