using System.Text;
using FinBackend.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var dbUrl = Environment.GetEnvironmentVariable("DATABASE_URL")
            ?? throw new Exception("No DATABASE_URL!");

var uri = new Uri(dbUrl);
var userInfo = uri.UserInfo.Split(':');

var dbStr = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
Console.WriteLine("URL: " + dbStr);

var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
                ?? throw new Exception("No JWT_SECRET!");

byte[] keyBytes;
try
{
    keyBytes = Convert.FromBase64String(jwtSecret);
}
catch (FormatException)
{
    keyBytes = Encoding.UTF8.GetBytes(jwtSecret);
}

if (keyBytes.Length < 16)
{
    throw new Exception($"JWT key is too short!");
}

var securityKey = new SymmetricSecurityKey(keyBytes);

builder.Services.AddDbContext<FinContext>(opts =>
    opts.UseNpgsql(dbStr));

builder.Services.AddSingleton<SecurityKey>(securityKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapFallbackToFile("/index.html");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");
