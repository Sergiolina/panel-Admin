using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace panel_Admin.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerCategorias()
    {
        var categorias = await _context.Categorias
            .OrderBy(c => c.Orden)
            .ToListAsync();

        return Ok(categorias);
    }
[Authorize(Roles = "Admin")]
[HttpPost]
public async Task<IActionResult> CrearCategoria(Categoria categoria)
{
    if (string.IsNullOrWhiteSpace(categoria.Nombre))
    {
        return BadRequest("El nombre de la categoría es obligatorio.");
    }

    _context.Categorias.Add(categoria);
    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(ObtenerCategorias),
        new { id = categoria.Id },
        categoria);
}
 [Authorize(Roles = "Admin")]
 [HttpPut("{id}")]
public async Task<IActionResult> ActualizarCategoria(int id, Categoria categoria)
{
    var categoriaExistente = await _context.Categorias.FindAsync(id);

    if (categoriaExistente == null)
    {
        return NotFound("Categoria no encontrado.");
    }

    if (string.IsNullOrWhiteSpace(categoria.Nombre))
    {
        return BadRequest("El nombre del producto es obligatorio.");
    }

    categoriaExistente.Nombre = categoria.Nombre;
    categoriaExistente.Descripcion = categoria.Descripcion;
    categoriaExistente.Orden = categoria.Orden;
   
    await _context.SaveChangesAsync();

    return Ok(categoriaExistente);
}
[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public async Task<IActionResult> EliminarCategoria(int id)
{
    var categoria = await _context.Categorias.FindAsync(id);

    if (categoria == null)
    {
        return NotFound("Categoría no encontrada.");
    }

    _context.Categorias.Remove(categoria);
    await _context.SaveChangesAsync();

    return NoContent();
}
}
