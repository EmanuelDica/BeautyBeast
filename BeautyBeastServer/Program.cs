using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using BeautyBeastServer.Services;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Server;
using BeautyBeastServer.Components;
using Blazored.LocalStorage;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add Blazored Local Storage service
//builder.Services.AddBlazoredLocalStorage();

// Add Razor Pages and Blazor services
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredLocalStorage();

// Add interactive Razor Components (SSR with optional interactivity)
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// Add HttpClient for API calls
var beautyBeastApiUrl = builder.Configuration["BeautyBeastApiUrl"] ?? throw new Exception("BeautyBeastApiUrl is not set.");
builder.Services.AddHttpClient("BeautyBeastApi", client =>
{
    client.BaseAddress = new Uri(beautyBeastApiUrl);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
});

// Register services
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ArtistService>();
builder.Services.AddScoped<ArtistAchievementService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<StatusService>();
builder.Services.AddScoped<TreatmentService>();
builder.Services.AddScoped<AuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Configure interactive Razor Components with server-side rendering
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();