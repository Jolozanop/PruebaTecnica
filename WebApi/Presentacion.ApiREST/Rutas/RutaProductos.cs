using Aplicacion.Core.Interfaces;
using Dominio.Principal.Entidades;
using Serilog;
using Serilog.Sinks.File;
using Serilog.Extensions.Logging;
using Aplicacion.Core;
using Microsoft.AspNetCore.Mvc;
using Aplicacion.Core.Especificacion.Productos;
using Presentacion.ApiREST.Middleware;
using Aplicacion.Core.Adaptadores;
using FluentValidation;

namespace Presentacion.ApiREST.Rutas;

public static class RutaProductos
{
    const string ENDPOINT = "/productos";
    public static IEndpointRouteBuilder AgregarRutasProductos(this IEndpointRouteBuilder app)
    {
        app.MapGet(ENDPOINT, GetAllProductos).WithTags(ENDPOINT);
        app.MapGet(ENDPOINT + "/{id}", GetProductosById).WithTags(ENDPOINT);
        app.MapPost(ENDPOINT, PostProductos).WithTags(ENDPOINT);
        app.MapPut(ENDPOINT + "/{id}", PutProductos).WithTags(ENDPOINT);
        app.MapDelete(ENDPOINT + "/{id}", DeleteProductos).WithTags(ENDPOINT);

        app.MapGet(ENDPOINT+"_Busqueda", BusquedaProductos).WithTags(ENDPOINT);

        //app.MapGet("/search", (ProductosParametros criteria) =>
        //{
        //    return $"Author: {criteria.Nombre}, Year published: {criteria.Descripcion}";
        //});

        return app;

        async Task<IResult> GetAllProductos(IUnitOfWork _unitOfWork)
            => Results.Ok((await _unitOfWork.Repositorio<Producto>().GetAllAsync()));


        async Task<IResult> GetProductosById(IUnitOfWork _unitOfWork, int id)
        {
            //logger.LogInformation("Consultar por ID {id}", id);
            Log.Information("Consultar producto por ID {id}", id);
            var prod = await _unitOfWork.Repositorio<Producto>().GetByIdAsync(id);
            return prod is null ? Results.NotFound() : Results.Ok(prod);
        }

        async Task<IResult> BusquedaProductos(IUnitOfWork _unitOfWork, ProductosParametros parametros)
        {
            var especificacion = new ProductosGeneral(parametros);
            var productos = await _unitOfWork.Repositorio<Producto>().GetAll_ConEspecificacion(especificacion);
            var conteoProductos = await _unitOfWork.Repositorio<Producto>().ConteoAsync(especificacion);

            var rounded = Math.Ceiling(Convert.ToDecimal(conteoProductos / parametros.TamanoPagina));
            var totalPaginas = Convert.ToInt32(rounded);

            if (productos is null || productos?.Count == 0)
                return Results.NotFound();

            return Results.Ok(
               new Paginacion<Producto>
               {
                   Count = conteoProductos,
                   Data = productos,
                   PageCount = totalPaginas,
                   PageIndex = parametros.IndicePagina,
                   PageSize = parametros.TamanoPagina
               }
           );
        }

        async Task<IResult> PostProductos(IUnitOfWork _unitOfWork, IValidator<In_ProductoDTO> _validator, In_ProductoDTO ProductoDTO)
        {
            var validationResult = _validator.Validate(ProductoDTO);

            if (!validationResult.IsValid)
                return Results.BadRequest(Results.ValidationProblem(validationResult.ToDictionary()));

            var ExisteCategoria = await _unitOfWork.Repositorio<Categoria>().GetByIdAsync(ProductoDTO.IdCategoria);
            if (ExisteCategoria is null)
                return Results.BadRequest("Categoria no existe!");

            Producto newProd = new Producto
            {
                IdProductoPk = 0,
                Nombre = ProductoDTO.Nombre,
                Descripcion = ProductoDTO.Descripcion,
                IdCategoriaFk = ProductoDTO.IdCategoria,
                Categoria = ExisteCategoria,
                ImagenUrl = ProductoDTO.ImagenUrl,
                FechaRegistro = DateTime.Now,
                Estado = true
            };


            //Usar UnitOfWork para confirmar datos inmediatamente segun estrategia 
            await _unitOfWork.Repositorio<Producto>().Agregar(newProd);

            //Usar UnitOfWork cuando sea necesario aplicar por carga de trabajo hacia la BD
            //_unitOfWork.Repositorio<Producto>().AddEntity(newProd);
            //await _unitOfWork.Complete();
            //--------------------------------------------------


            return Results.Ok(newProd);
        }


        async Task<IResult> PutProductos(IUnitOfWork _unitOfWork, IValidator<In_ProductoDTO> _validator, int id, In_ProductoDTO ProductoDTO)
        {
            var validationResult = _validator.Validate(ProductoDTO);

            if (!validationResult.IsValid)
                return Results.BadRequest(Results.ValidationProblem(validationResult.ToDictionary()));

            var Existeproducto = await _unitOfWork.Repositorio<Producto>().GetByIdAsync(id);
            if (Existeproducto is not null)
            {
                Existeproducto.Nombre = ProductoDTO.Nombre;
                Existeproducto.Descripcion = ProductoDTO.Descripcion;
                Existeproducto.IdCategoriaFk = ProductoDTO.IdCategoria;
                Existeproducto.ImagenUrl = ProductoDTO.ImagenUrl;
                Existeproducto.Estado = true;

                await _unitOfWork.Repositorio<Producto>().Actualizar(Existeproducto);
                return Results.Ok();
            }
            else
                return Results.NotFound();
        }


        async Task<IResult> DeleteProductos(IUnitOfWork _unitOfWork, int id)
        {
            var Existeproducto = await _unitOfWork.Repositorio<Producto>().GetByIdAsync(id);
            if (Existeproducto is not null)
            {
                await _unitOfWork.Repositorio<Producto>().Eliminar(Existeproducto);
                return Results.Ok();
            }
            else
                return Results.NotFound();
        }

    }
}
