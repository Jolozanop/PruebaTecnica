using System.ComponentModel.DataAnnotations;

namespace Dominio.Principal.Entidades;


public class Categoria
{
    [Key]
    public int IdCategoriaPk { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public bool? Estado { get; set; }
}
