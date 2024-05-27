namespace FechasFestivas.Core.Interfaces.Repositorios
{
    public interface IFestivosRepositorio
    {
        Task<IEnumerable<Festivos>> ObtenerTodos();

        Task<Festivos> Obtener(int id);

        Task<Festivos> Agregar(Festivos Festivos);

        Task<Festivos> Modificar(Festivos Festivos);

        Task<bool> Eliminar(int id);
    }
}
