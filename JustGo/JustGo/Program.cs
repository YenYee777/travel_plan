using JustGo.Data;
using JustGo.Models;
using JustGo.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var TravelWindows = builder.Configuration.GetConnectionString("TravelWindows");
var TravelPssP = builder.Configuration.GetConnectionString("TravelPssP");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(TravelPssP));
builder.Services.AddTransient<IDbConnection>(db => new SqlConnection(TravelPssP));
builder.Services.AddTransient<IPlaceWeatherRepostiory, PlaceWeatherRepostiory>();
builder.Services.AddTransient<IScheduleRepostioy, ScheduleRepostioy>();
builder.Services.AddTransient<IBlogRepostioy, BlogRepostioy>();



builder.Services.AddDbContext<TravelContext>(o => o.UseSqlServer(TravelPssP));
//連線字串替換
//地端連線字串TravelWindows,雲端連線字串TravelPssP
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
