using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace panel_Admin.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerProductos()
    {
        var productos = await _context.Productos.ToListAsync();

        return Ok(productos);
    }

    [HttpPost]
    public async Task<IActionResult> CrearProducto(Producto producto)
    {
        if (string.IsNullOrWhiteSpace(producto.Nombre))
        {
            return BadRequest("El nombre del producto es obligatorio.");
        }

        if (producto.Precio < 0)
        {
            return BadRequest("El precio no puede ser negativo.");
        }

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(ObtenerProductos),
            new { id = producto.Id },
            producto);
    }
  [HttpPut("{id}")]
public async Task<IActionResult> ActualizarProducto(int id, Producto producto)
{
    var productoExistente = await _context.Productos.FindAsync(id);

    if (productoExistente == null)
    {
        return NotFound("Producto no encontrado.");
    }

    if (string.IsNullOrWhiteSpace(producto.Nombre))
    {
        return BadRequest("El nombre del producto es obligatorio.");
    }

    if (producto.Precio < 0)
    {
        return BadRequest("El precio no puede ser negativo.");
    }

    productoExistente.Nombre = producto.Nombre;
    productoExistente.Precio = producto.Precio;
    productoExistente.Descripcion = producto.Descripcion;
    productoExistente.Categoria = producto.Categoria;
    productoExistente.ImagenUrl = producto.ImagenUrl;
    productoExistente.Disponible = producto.Disponible;

    await _context.SaveChangesAsync();

    return Ok(productoExistente);
}
}