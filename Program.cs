using Microsoft.EntityFrameworkCore;
using panel_Admin;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=panel-admin.db"));

var app = builder.Build();

app.MapGet("/", () => "API del Panel Admin funcionando");

app.Run();// prueba de sincronizacion
