namespace FechasFestivas.Aplicacion
{
    public class FestivosServicio : IFestivoServicio
    {
        private readonly IFestivosRepositorio _repositorio;

        public FestivosServicio(IFestivosRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Festivos> Obtener(int id)
        {
            return await _repositorio.Obtener(id);
        }

        public async Task<IEnumerable<Festivos>> ObtenerTodos()
        {
            return await _repositorio.ObtenerTodos();
        }


        public async Task<IEnumerable<Festivos>> validarAnho(int anho)
        {
            IEnumerable<Festivos> retorno = new List<Festivos>();

            ValidarAnho(anho);

            var festivos = await ObtenerTodos();

            //Tipo uno
            retorno = festivos.Where(e => e.IdTipo == TipoFestivo.Fijo.GetHashCode()).ToList();


            //Tipo dos

            var festivosLunes = festivos.Where(e => e.IdTipo == TipoFestivo.LeyPuenteFestivo.GetHashCode()).ToList();

            foreach (var festivoLunes in festivosLunes)
            {
                DateTime fechaFestivoLunes = new DateTime(anho, festivoLunes.Mes, festivoLunes.Dia);

                fechaFestivoLunes = GetNextMonday(fechaFestivoLunes);

                festivoLunes.Dia = fechaFestivoLunes.Day;
                festivoLunes.Mes = fechaFestivoLunes.Month;

                retorno = retorno.Append(festivoLunes);
            }

            //Tipo tres
            var festivosPascua = festivos.Where(e => e.IdTipo == TipoFestivo.BasadoPascua.GetHashCode()).ToList();
            foreach (var festivoPascua in festivosPascua)
            {
                var domigoPascua = CalcularDomingoPascua(anho);

                domigoPascua = domigoPascua.AddDays(festivoPascua.DiasPascua);

                festivoPascua.Dia = domigoPascua.Day;
                festivoPascua.Mes = domigoPascua.Month;

                retorno = retorno.Append(festivoPascua);
            }

            //Tipo cuatro
            var festivosPascuaPasaLunes = festivos.Where(e => e.IdTipo == TipoFestivo.BasadoPascualYLeyPuenteFes.GetHashCode()).ToList();
            foreach (var festivoPascuaLunes in festivosPascuaPasaLunes)
            {
                var domigoPascua = CalcularDomingoPascua(anho);

                domigoPascua = domigoPascua.AddDays(festivoPascuaLunes.DiasPascua);

                domigoPascua = GetNextMonday(domigoPascua);

                festivoPascuaLunes.Dia = domigoPascua.Day;
                festivoPascuaLunes.Mes = domigoPascua.Month;

                retorno = retorno.Append(festivoPascuaLunes);
            }

            return retorno.OrderBy(e => e.Id);
        }

        private void ValidarAnho(int anho)
        {
            if (anho < 1900)
                throw new Exception("El año minimo es 1900");

            if (anho > 2050)
                throw new Exception("El año maximo es 2050");
        }


        public async Task<bool> Verificar(int ano, int mes, int dia)
        {
            var festivos = await ObtenerTodos();
            ValidarFecha(ano, mes, dia);

            // Verificación de festivos tipo uno
            if (festivos.Any(f => f.IdTipo == 1 && f.Dia == dia && f.Mes == mes))
                return true;

            // Verificación de festivos tipo dos
            if (VerificarFestivoTipoDos(festivos, ano, mes, dia))
                return true;

            // Verificación de festivos tipo tres
            if (VerificarFestivoTipoTres(festivos, ano, mes, dia))
                return true;

            // Verificación de festivos tipo cuatro
            if (VerificarFestivoTipoCuatro(festivos, ano, mes, dia))
                return true;

            // Verificación de Domingo de Ramos
            if (VerificarDomingoRamos(ano, mes, dia))
                return true;

            return false;
        }

        private bool VerificarFestivoTipoDos(IEnumerable<Festivos> festivos, int ano, int mes, int dia)
        {
            var festivosLunes = festivos.Where(f => f.IdTipo == 2).ToList();

            foreach (var festivoLunes in festivosLunes)
            {
                DateTime fechaFestivoLunes = new DateTime(ano, festivoLunes.Mes, festivoLunes.Dia);
                fechaFestivoLunes = GetNextMonday(fechaFestivoLunes);

                if (fechaFestivoLunes.Day == dia && fechaFestivoLunes.Month == mes)
                    return true;
            }

            return false;
        }

        private bool VerificarFestivoTipoTres(IEnumerable<Festivos> festivos, int ano, int mes, int dia)
        {
            var festivosPascua = festivos.Where(f => f.IdTipo == 3).ToList();

            foreach (var festivoPascua in festivosPascua)
            {
                var domingoPascua = CalcularDomingoPascua(ano);
                domingoPascua = domingoPascua.AddDays(festivoPascua.DiasPascua);

                if (mes == domingoPascua.Month && dia == domingoPascua.Day)
                    return true;
            }

            return false;
        }

        private bool VerificarFestivoTipoCuatro(IEnumerable<Festivos> festivos, int ano, int mes, int dia)
        {
            var festivosPascuaLunes = festivos.Where(f => f.IdTipo == 4).ToList();

            foreach (var festivoPascuaLunes in festivosPascuaLunes)
            {
                var domigoPascua = CalcularDomingoPascua(ano);
                domigoPascua = domigoPascua.AddDays(festivoPascuaLunes.DiasPascua);
                domigoPascua = GetNextMonday(domigoPascua);

                if (domigoPascua.Day == dia && domigoPascua.Month == mes)
                    return true;
            }

            return false;
        }

        private bool VerificarDomingoRamos(int ano, int mes, int dia)
        {
            var domingoRamos = CalcularDomingoRamos(ano);
            return mes == domingoRamos.Month && dia == domingoRamos.Day;
        }

        private DateTime CalcularDomingoRamos(int ano)
        {
            var a = ano % 19;
            var b = ano % 4;
            var c = ano % 7;
            var d = ((19 * a) + 24) % 30;

            var dias = d + ((2 * b) + (4 * c) + (6 * d) + 5) % 7;

            //mes marzo
            var domingoRamos = new DateTime(ano, 3, 15);
            domingoRamos = domingoRamos.AddDays(dias);

            return domingoRamos;
        }

        private DateTime CalcularDomingoPascua(int ano)
        {
            var a = ano % 19;
            var b = ano % 4;
            var c = ano % 7;
            var d = ((19 * a) + 24) % 30;

            var dias = d + ((2 * b) + (4 * c) + (6 * d) + 5) % 7;

            //mes marzo
            var domingoPascua = new DateTime(ano, 3, 22);
            domingoPascua = domingoPascua.AddDays(dias);

            return domingoPascua;
        }

        private DateTime GetNextMonday(DateTime date)
        {
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)date.DayOfWeek + 7) % 7;
            return date.AddDays(daysUntilMonday);
        }

        private void ValidarFecha(int ano, int mes, int dia)
        {
            DateTime fecha;
            try
            {
                fecha = new DateTime(ano, mes, dia);
            }
            catch (Exception)
            {
                throw new Exception("Fecha invalida.");
            }

            if (fecha < new DateTime(1900, 1, 1))
            {
                throw new Exception("No se puede ingresar fecha menor al año 1900");
            }

            if (fecha > new DateTime(2050, 1, 1))
            {
                throw new Exception("No se puede ingresar fecha mayor al año 2050");
            }
        }


        public enum TipoFestivo
        {
            Fijo = 1,
            LeyPuenteFestivo = 2,
            BasadoPascua = 3,
            BasadoPascualYLeyPuenteFes = 4
        }



    }
}
