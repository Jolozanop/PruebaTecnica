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

public static class RutaCategorias
{
    const string ENDPOINT = "/categorias";
    public static IEndpointRouteBuilder AgregarRutasCategorias(this IEndpointRouteBuilder app)
    {
        app.MapGet(ENDPOINT, GetAllCategorias).WithTags(ENDPOINT);
        app.MapGet(ENDPOINT + "/{id}", GetCategoriasById).WithTags(ENDPOINT);
        app.MapPost(ENDPOINT, PostCategorias).WithTags(ENDPOINT);
        app.MapPut(ENDPOINT + "/{id}", PutCategorias).WithTags(ENDPOINT);
        app.MapDelete(ENDPOINT + "/{id}", DeleteCategorias).WithTags(ENDPOINT);

        return app;

        async Task<IResult> GetAllCategorias(IUnitOfWork _unitOfWork)
            => Results.Ok((await _unitOfWork.Repositorio<Categoria>().GetAllAsync()));


        async Task<IResult> GetCategoriasById(IUnitOfWork _unitOfWork, int id)
        {
            //logger.LogInformation("Consultar por ID {id}", id);
            Log.Information("Consultar Categoria por ID {id}", id);
            var prod = await _unitOfWork.Repositorio<Categoria>().GetByIdAsync(id);
            return prod is null ? Results.NotFound() : Results.Ok(prod);
        }


        async Task<IResult> PostCategorias(IUnitOfWork _unitOfWork, IValidator<In_CategoriaDTO> _validator, In_CategoriaDTO CategoriaDTO)
        {
            var validationResult = _validator.Validate(CategoriaDTO);

            if (!validationResult.IsValid)
                return Results.BadRequest(Results.ValidationProblem(validationResult.ToDictionary()));

            Categoria newCat = new Categoria
            {
                IdCategoriaPk = 0,
                Nombre = CategoriaDTO.Nombre,
                Descripcion = CategoriaDTO.Descripcion,
                Estado = true
            };


            //Usar UnitOfWork para confirmar datos inmediatamente segun estrategia 
            //await _unitOfWork.Repositorio<Categoria>().Agregar(newCat);

            //Usar UnitOfWork cuando sea necesario aplicar por carga de trabajo hacia la BD
            _unitOfWork.Repositorio<Categoria>().AddEntity(newCat);
            //...
            await _unitOfWork.Complete();
            //--------------------------------------------------


            return Results.Ok(newCat);
        }


        async Task<IResult> PutCategorias(IUnitOfWork _unitOfWork, int id, IValidator<In_CategoriaDTO> _validator, In_CategoriaDTO CategoriaDTO)
        {
            var validationResult = _validator.Validate(CategoriaDTO);

            if (!validationResult.IsValid)
                return Results.BadRequest(Results.ValidationProblem(validationResult.ToDictionary()));

            var ExisteCategoria = await _unitOfWork.Repositorio<Categoria>().GetByIdAsync(id);
            if (ExisteCategoria is not null)
            {
                ExisteCategoria.Nombre = CategoriaDTO.Nombre;
                ExisteCategoria.Descripcion = CategoriaDTO.Descripcion;
                ExisteCategoria.Estado = true;

                await _unitOfWork.Repositorio<Categoria>().Actualizar(ExisteCategoria);
                return Results.Ok();
            }
            else
                return Results.NotFound();
        }


        async Task<IResult> DeleteCategorias(IUnitOfWork _unitOfWork, int id)
        {
            var ExisteCategoria = await _unitOfWork.Repositorio<Categoria>().GetByIdAsync(id);
            if (ExisteCategoria is not null)
            {
                await _unitOfWork.Repositorio<Categoria>().Eliminar(ExisteCategoria);
                return Results.Ok();
            }
            else
                return Results.NotFound();
        }

    }
}
