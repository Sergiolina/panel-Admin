using Microsoft.EntityFrameworkCore;
using panel_Admin;
using panel_Admin.Services;
using panel_Admin.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_sqlite3());

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Catalogo", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
ValidAudience = builder.Configuration["Jwt:Audience"],

IssuerSigningKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<PasswordService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=panel-admin.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var passwordService = scope.ServiceProvider.GetRequiredService<PasswordService>();

    await DbInitializer.InicializarAsync(context, passwordService);
}
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();

app.UseCors("Catalogo");

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "API del Panel Admin funcionando");

app.Run();// funciona
