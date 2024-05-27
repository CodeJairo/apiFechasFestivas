namespace FechasFestivas.Infraestructura.Repositorio
{
    public class FestivosRepositorio : IFestivosRepositorio
    {
        private FechasFestivasContext context;


        public FestivosRepositorio(FechasFestivasContext context)
        {
            this.context = context;
        }

        public async Task<Festivos> Agregar(Festivos Festivos)
        {
            context.Festivos.Add(Festivos);
            await context.SaveChangesAsync();
            return Festivos;
        }

        public async Task<bool> Eliminar(int Id)
        {
            var campeonatoExistente = await context.Festivos.FindAsync(Id);
            if (campeonatoExistente == null)
            {
                return false;
            }

            context.Festivos.Remove(campeonatoExistente);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Festivos> Modificar(Festivos Festivos)
        {
            var campeonatoExistente = await context.Festivos.FindAsync(Festivos.Id);
            if (campeonatoExistente == null)
            {
                return null;
            }
            context.Entry(campeonatoExistente).CurrentValues.SetValues(Festivos);
            await context.SaveChangesAsync();
            return await context.Festivos.FindAsync(Festivos.Id);
        }

        public async Task<Festivos> Obtener(int Id)
        {
            return await context.Festivos.FindAsync(Id);
        }

        public async Task<IEnumerable<Festivos>> ObtenerTodos()
        {
            return await context.Festivos.ToArrayAsync();
        }
    }
}
