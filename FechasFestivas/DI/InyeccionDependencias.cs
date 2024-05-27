namespace FechasFestivas.Presentacion.DI
{
    public static class InyeccionDependencias
    {
        public static IServiceCollection AgregarDependencias(this IServiceCollection servicios, IConfiguration configuracion)
        {
            //Agregar el DBContext
            servicios.AddDbContext<FechasFestivasContext>(opcionesConstruccion =>
            {
                opcionesConstruccion.UseSqlServer(configuracion.GetConnectionString("Festivos"));
            });

            //Agregar los repositorios
            servicios.AddTransient<ITipoRepositorio, TipoRepositorio>();
            servicios.AddTransient<IFestivosRepositorio, FestivosRepositorio>();

            //Agregar los servicios
            servicios.AddTransient<ITipoServicio, TipoServicio>();
            servicios.AddTransient<IFestivoServicio, FestivosServicio>();


            return servicios;
        }
    }
}
