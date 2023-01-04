using System.ComponentModel.DataAnnotations;

namespace Dominio.Principal.Entidades;


public class Producto
{
    [Key]
    public int IdProductoPk { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }

    public int IdCategoriaFk { get; set; }
    public virtual Categoria Categoria { get; set; }

    public string? ImagenUrl { get; set; }
    public DateTime FechaRegistro { get; set; }
    public bool? Estado { get; set; }
}
