using Microsoft.EntityFrameworkCore;
using BeautyBeastApi.Data;
using BeautyBeastApi.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// register connection str
var connectionString = builder.Configuration.GetConnectionString("BeautyBeast");
builder.Services.AddSqlite<BeautyBeastContext>(connectionString);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

// JWT Configuration
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

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// endpoints
app.MapUsersEndpoints();
app.MapBookingsEndpoints();
app.MapArtistsEndpoints();
app.MapClientsEndpoints();
app.MapCommentsEndpoints();
app.MapPostsEndpoints();
app.MapTreatmentsEndpoints();
app.MapStatusEndpoints();

app.MigrateDb();

app.Run();