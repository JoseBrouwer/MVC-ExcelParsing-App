using ExcelParsing.Data;
using ExcelParsing.Middleware;
using ExcelParsing.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add Db to Services
builder.Services.AddDbContext<AppDbContext>();

// Register the ExcelService
builder.Services.AddScoped<ExcelService>();

var app = builder.Build();

// Seed the database for testing
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    DatabaseSeeder.Seed(services);
//}

// Add the custom error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ExcelParsing}/{action=Index}/{id?}");

app.Run();
