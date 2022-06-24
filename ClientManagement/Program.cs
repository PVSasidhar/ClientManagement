
using ApplicationCore.Clients;
using ApplicationCore.Core;
using Infrastructure; 
 
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SQLServer");

builder.Services.AddConnections();
builder.Services.AddSingleton((System.Func<System.IServiceProvider, IClientContext>)(_ => new ClientContext(connectionString)));
builder.Services.AddSingleton((System.Func<System.IServiceProvider, IFileExporter>)(_ => new FileExporter(connectionString)));
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
