namespace FechasFestivas.Core.Interfaces.Servicios
{
    public interface IFestivoServicio
    {
        Task<IEnumerable<Festivos>> ObtenerTodos();
        Task<bool> Verificar(int ano, int mes, int dia);

        Task<Festivos>Obtener(int id);
        Task<IEnumerable<Festivos>> validarAnho(int nano);
    }
}
