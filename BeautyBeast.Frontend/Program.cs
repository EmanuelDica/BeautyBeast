using System.Net.Http.Headers;
using BeautyBeast.Frontend.Components;
using BeautyBeast.Frontend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();


var app = builder.Build();

var beautyBeastApiUrl = builder.Configuration["BeautyBeastApiUrl"]??
    throw new Exception("beautyBeastApiUrl is not set.");

// Register API HttpClient properly using AddHttpClient<T>()
builder.Services.AddHttpClient<ClientService>(client =>
{
    client.BaseAddress = new Uri(beautyBeastApiUrl);
});

builder.Services.AddHttpClient<UserService>(client =>
{
    client.BaseAddress = new Uri(beautyBeastApiUrl);
});

builder.Services.AddHttpClient<ArtistService>(client =>
{
    client.BaseAddress = new Uri(beautyBeastApiUrl);
});

builder.Services.AddHttpClient<ArtistAchievementService>(client =>
{
    client.BaseAddress = new Uri(beautyBeastApiUrl);
});

builder.Services.AddHttpClient<BookingService>(client =>
{
    client.BaseAddress = new Uri(beautyBeastApiUrl);
});

builder.Services.AddHttpClient<CommentService>(client =>
{
    client.BaseAddress = new Uri(beautyBeastApiUrl);
});

builder.Services.AddHttpClient<PostService>(client =>
{
    client.BaseAddress = new Uri(beautyBeastApiUrl);
});

builder.Services.AddHttpClient<StatusService>(client =>
{
    client.BaseAddress = new Uri(beautyBeastApiUrl);
});

builder.Services.AddHttpClient<TreatmentService>(client =>
{
    client.BaseAddress = new Uri(beautyBeastApiUrl);
});

builder.Services.AddHttpClient<AuthenticationService>(client =>
{
    client.BaseAddress = new Uri(beautyBeastApiUrl);
});


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
