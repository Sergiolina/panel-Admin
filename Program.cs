using Microsoft.EntityFrameworkCore;
using panel_Admin;

SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_sqlite3());

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=panel-admin.db"));

var app = builder.Build();

app.MapControllers();

app.MapGet("/", () => "API del Panel Admin funcionando");

app.Run();// funciona
