using MudBlazor.Services;
using BeautyBeastFrontend.Components;
using BeautyBeastFrontend.Services;
using System.Net.Http.Headers;
using Blazored.LocalStorage;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();
// Add Blazored Local Storage service
builder.Services.AddBlazoredLocalStorage();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var beautyBeastApiUrl = builder.Configuration["BeautyBeastApiUrl"]??
    throw new Exception("beautyBeastApiUrl is not set.");

builder.Services.AddScoped(sp => 
{
    var httpClient = new HttpClient 
    {
         BaseAddress = new Uri(beautyBeastApiUrl) 
    };
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
    
    return httpClient;
});

// registered services
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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
