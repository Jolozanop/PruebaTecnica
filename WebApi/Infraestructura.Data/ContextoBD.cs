using Dominio.Principal.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Data;

public class ContextoBD : DbContext
{
    public ContextoBD(DbContextOptions<ContextoBD> options) : base(options)
    { }

    public DbSet<Producto> Producto { get; set; }
    public DbSet<Categoria> Categoria { get; set; }
}