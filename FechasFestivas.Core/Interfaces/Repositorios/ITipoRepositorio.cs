namespace FechasFestivas.Core.Interfaces.Repositorios
{
    public interface ITipoRepositorio
    {
        Task<IEnumerable<Tipo>> ObtenerTodos();

        Task<Tipo> Obtener(int id);

        Task<Tipo> Agregar(Tipo Tipo);

        Task<Tipo> Modificar(Tipo Tipo);

        Task<bool> Eliminar(int id);
    }
}
