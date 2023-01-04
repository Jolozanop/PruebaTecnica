using Aplicacion.Core;
using FluentValidation;

namespace Applicacion.Core.Validadores;

public class CategoriaValidador : AbstractValidator<In_CategoriaDTO>
{
    public CategoriaValidador()
    {
        RuleFor(b => b.Nombre)
                .NotNull().WithMessage("El parametro nombre es obligatorio")
                .MinimumLength(3).WithMessage("El Nombre debe contener al menos 3 carácteres");
    }
}
