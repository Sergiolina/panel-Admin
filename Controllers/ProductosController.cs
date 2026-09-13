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
}
