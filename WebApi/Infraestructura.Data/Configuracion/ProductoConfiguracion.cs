using Dominio.Principal.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configuracion
{
    public class ProductoConfiguracion: IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.Property(p => p.Nombre).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Descripcion).IsRequired().HasMaxLength(750);
            builder.Property(p => p.IdCategoriaFk).IsRequired();
            builder.HasOne(p => p.Categoria).WithMany().HasForeignKey(prop => prop.IdCategoriaFk).OnDelete(DeleteBehavior.Restrict);
            builder.Property(p => p.FechaRegistro).IsRequired();
        }
    }
}
