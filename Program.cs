using Microsoft.EntityFrameworkCore;
using ShimanoTask.Models;
using ShimanoTask.Data;
using System;
using Microsoft.Extensions.DependencyInjection;
using ShimanoTask;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MyAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Calling DataSeeder Class
builder.Services.AddTransient<DataSeeder>();

var app = builder.Build();

// check if the args after "dotnet run args" containing the seeding script
if(args.Length == 1 && args[0].ToLower() == "seeddata"){
    SeedData(app);
}

// the seeding function
void SeedData (IHost app) {
    var ScopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = ScopedFactory.CreateScope()) {
        var service = scope.ServiceProvider.GetService<DataSeeder>();
        service.Seed();
    }
}   

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// This is for my seeding

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
