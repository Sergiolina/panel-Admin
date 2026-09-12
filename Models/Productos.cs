namespace panel_Admin;

public class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public decimal Precio { get; set; }

    public string Descripcion { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public string ImagenUrl { get; set; } = string.Empty;

    public bool Disponible { get; set; } = true;
}