using Microsoft.EntityFrameworkCore;
using panel_Admin.Services;

namespace panel_Admin.Data;

public static class DbInitializer
{
    public static async Task InicializarAsync(
        AppDbContext context,
        PasswordService passwordService)
    {
	await context.Database.MigrateAsync();

	if (await context.Usuarios.AnyAsync())
        {
            return;
        }

        var administrador = new Usuario
        {
            NombreUsuario = "admin"
        };

        administrador.PasswordHash = passwordService.HashearPassword(
            administrador,
            "Admin1234!"
        );

        context.Usuarios.Add(administrador);

        await context.SaveChangesAsync();
    }
}
