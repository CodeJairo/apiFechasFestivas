namespace FechasFestivas.Presentacion.Controllers
{
    [ApiController]
    [Route("api/tipos")]
    public class TipoControlador : ControllerBase
    {
        private ITipoServicio servicio;

        public TipoControlador(ITipoServicio servicio)
        {
            this.servicio = servicio;
        }
    }
}