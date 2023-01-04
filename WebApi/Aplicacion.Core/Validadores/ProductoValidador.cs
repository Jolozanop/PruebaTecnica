using Aplicacion.Core;
using FluentValidation;

namespace Applicacion.Core.Validadores;

public class ProductoValidador : AbstractValidator<In_ProductoDTO>
{
    public ProductoValidador()
    {
        RuleFor(b => b.Nombre)
                .NotNull().WithMessage("El parametro nombre es obligatorio")
                .MinimumLength(3).WithMessage("El Nombre debe contener al menos 3 carácteres");

        RuleFor(b => b.Descripcion)
                .NotNull().WithMessage("El parametro Descripcion es obligatorio");

        RuleFor(b => b.IdCategoria)
                .NotNull().WithMessage("El parametro Categoria es obligatorio")
                .GreaterThan(0);

        RuleFor(b => b.ImagenUrl)
                .NotNull().WithMessage("El parametro ImagenUrl es obligatorio");
    }
}
