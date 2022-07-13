
using ApplicationCore.ClientAddress;
using ApplicationCore.Clients;
using ApplicationCore.Core;
using Infrastructure;
using Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SQLServer");

var DBConnectionService1 = new DBConnectionService(connectionString); 
builder.Services.AddConnections();
 
builder.Services.AddSingleton<IDBConnectionService>(DBConnectionService1) ;
builder.Services.AddScoped<IClientContext,ClientContext > () ;
builder.Services.AddScoped<IFileExporter,FileExporter> () ;
builder.Services.AddScoped<IAddressContext, AddressContext>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
