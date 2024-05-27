namespace FechasFestivas.Presentacion.Controllers
{
    [ApiController]
    [Route("festivos")]

    public class FestivosControlador : ControllerBase
    {
        private IFestivoServicio _servicio;

        public FestivosControlador(IFestivoServicio servicio)
        {
            this._servicio = servicio;
        }

        [HttpGet("validar/{ano}/{mes}/{dia}")]
        public async Task<ActionResult<IEnumerable<Tipo>>> Verificar([FromRoute] int ano, [FromRoute] int mes, [FromRoute] int dia)
        {
            try
            {
                try
                {
                    DateTime fecha = new DateTime(ano, mes, dia);
                }
                catch (Exception)
                {
                    return BadRequest("La fecha no es valida.");
                }

                var esFestivo = await _servicio.Verificar(ano, mes, dia);

                return Ok(esFestivo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("validarAnho/{ano}")]
        public async Task<ActionResult<IEnumerable<Tipo>>> validarAnho([FromRoute] int ano)
        {
            try
            {
                var festivosNano = await _servicio.validarAnho(ano);

                return Ok(festivosNano);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
