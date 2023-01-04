namespace Aplicacion.Core;

public record In_ProductoDTO(string Nombre, string Descripcion, int IdCategoria, string ImagenUrl);
public record Out_ProductoDTO(int IdProducto, string Nombre, string Descripcion, string Categoria, string ImagenUrl);
public record In_CategoriaDTO(string Nombre, string Descripcion);
public record Out_CategoriaDTO(int IdCategoria, string Nombre, string Descripcion);