namespace FechasFestivas.Infraestructura.Repositorio
{
    public class TipoRepositorio : ITipoRepositorio
    {
        private readonly FechasFestivasContext context;

        public TipoRepositorio(FechasFestivasContext context,
            IMapper mapper)
        {
            this.context = context;
        }

        public async Task<Tipo> Agregar(Tipo Tipo)
        {
            context.Tipos.Add(Tipo);
            await context.SaveChangesAsync();
            return Tipo;
        }

        public async Task<bool> Eliminar(int Id)
        {
            var seleccionExistente = await context.Tipos.FindAsync(Id);
            if (seleccionExistente == null)
            {
                return false;
            }

            context.Tipos.Remove(seleccionExistente);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Tipo> Modificar(Tipo Tipo)
        {
            var seleccionExistente = await context.Tipos.FindAsync(Tipo.Id);
            if (seleccionExistente == null)
            {
                return null;
            }
            context.Entry(seleccionExistente).CurrentValues.SetValues(Tipo);
            await context.SaveChangesAsync();
            return await context.Tipos.FindAsync(Tipo.Id);
        }

        public async Task<Tipo> Obtener(int Id)
        {
            return await context.Tipos.FindAsync(Id);
        }

        public async Task<IEnumerable<Tipo>> ObtenerTodos()
        {
            return await context.Tipos.ToArrayAsync();
        }



    }
}
