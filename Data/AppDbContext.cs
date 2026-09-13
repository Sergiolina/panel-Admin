using Microsoft.EntityFrameworkCore;

namespace panel_Admin;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Producto> Productos { get; set; }
  public DbSet<Usuario> Usuarios { get; set; }
}