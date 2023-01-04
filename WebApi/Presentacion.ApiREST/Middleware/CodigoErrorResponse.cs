namespace Presentacion.ApiREST.Middleware;

public class CodigoErrorResponse
{
    public CodigoErrorResponse(int codigoEstado, string mensaje = null)
    {
        codigo = codigoEstado;
        this.mensaje = mensaje ?? GetDefaultMessageStatusCode(codigoEstado);
    }

    private string GetDefaultMessageStatusCode(int codigoEstado)
    {
        return codigoEstado switch
        {
            400 => "El Request enviado tiene errores",
            401 => "No tiene autorizacion para este recurso",
            404 => "El recurso no se encuentra disponible o no existe",
            500 => "Se produjo un error en el servidor",
            200 => "Registro existente",
            _ => null!
        };
    }

    public int codigo { get; set; }
    public string mensaje { get; set; }
}
