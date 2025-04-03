using Microsoft.EntityFrameworkCore;
using BeautyBeastApi.Data;
using BeautyBeastApi.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("BeautyBeast");
builder.Services.AddDbContext<BeautyBeastContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

var jwtKey = builder.Configuration["Jwt:Key"] ?? "supersecretkey";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "BeautyBeastApi";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(); 
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BeautyBeastContext>();
    dbContext.Database.Migrate(); 
}

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.MapUsersEndpoints();
app.MapBookingsEndpoints();
app.MapArtistsEndpoints();
app.MapClientsEndpoints();
app.MapCommentsEndpoints();
app.MapPostsEndpoints();
app.MapTreatmentsEndpoints();
app.MapStatusEndpoints();
app.MapArtistAchievementsEndpoints();

app.Run();