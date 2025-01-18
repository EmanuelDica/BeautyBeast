using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BeautyBeast;
using MudBlazor.Services;
using BeautyBeastApp.Services;
using BeautyBeastApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddScoped<PostRepository>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<BeautyBeastDbContext>();
builder.Services.AddMudServices();
builder.Services.AddDbContext<BeautyBeastDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleDbConnection")));

await builder.Build().RunAsync();
