using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EventEase3.Data;
using EventEase3.Models;
using Azure.Storage.Blobs;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EventEase3Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EventEase3Context") ?? throw new InvalidOperationException("Connection string 'EventEase3Context' not found.")));

//Configure Azure Blob Storage
builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new BlobServiceClient(config["AzureBlobStorage:ConnectionString"]);
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
