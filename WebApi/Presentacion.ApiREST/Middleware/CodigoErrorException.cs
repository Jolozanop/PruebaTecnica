namespace Presentacion.ApiREST.Middleware
{
    public class CodigoErrorException : CodigoErrorResponse
    {
        public CodigoErrorException(int codigoEstado, string mensaje = null, string detalles = null) : base(codigoEstado, mensaje)
        {
            Detalles = detalles;
        }

        public string Detalles { get; set; }
    }
}
