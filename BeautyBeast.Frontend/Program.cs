using System.Net.Http.Headers;
using BeautyBeast.Frontend.Components;
using BeautyBeast.Frontend.Services;
using Blazored.LocalStorage;

var builder = WebApplication.CreateBuilder(args);

// Ensure API URL is set
var beautyBeastApiUrl = builder.Configuration["BeautyBeastApiUrl"] ??
    throw new Exception("BeautyBeastApiUrl is not set.");

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register API HttpClient once and reuse it for multiple services
builder.Services.AddHttpClient("BeautyBeastApi", client =>
{
    client.BaseAddress = new Uri(beautyBeastApiUrl);
});

// Register Services with injected HttpClient
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BeautyBeastApi"));
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

// Register Local Storage
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredLocalStorageAsSingleton();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Ensure static files like CSS & JS load properly
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();