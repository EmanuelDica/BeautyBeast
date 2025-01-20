using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using BeautyBeastServer.Data;
using BeautyBeastServer.Services;
using BeautyBeastServer.Components;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorComponents()
builder.Services.AddScoped<PostRepository>();
    .AddInteractiveServerComponents();
builder.Services.AddDbContext<BeautyBeastDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleDbConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
