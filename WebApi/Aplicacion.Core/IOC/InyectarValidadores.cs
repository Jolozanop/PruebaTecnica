using Aplicacion.Core;
using Applicacion.Core.Validadores;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Applicacion.Core.IOC
{
    public static class InyectarValidadores
    {
        public static IServiceCollection AgregarValidadores(this IServiceCollection services)
        {
            services.AddScoped<IValidator<In_ProductoDTO>, ProductoValidador>();
            services.AddScoped<IValidator<In_CategoriaDTO>, CategoriaValidador>();

            return services;
        }
    }
}
