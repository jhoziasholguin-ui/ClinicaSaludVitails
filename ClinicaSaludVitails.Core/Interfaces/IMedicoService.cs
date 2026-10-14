using ClinicaSaludVitails.Models;

namespace ClinicaSaludVitails.Core.Interfaces
{
    public interface IMedicoService
    {
        Task<List<Medico>> ObtenerTodosAsync();

        Task<Medico?> ObtenerPorIdAsync(int id);

        Task<List<Especialidad>> ObtenerEspecialidadesAsync();

        Task CrearAsync(Medico medico);

        Task ActualizarAsync(Medico medico);

        Task<string?> EliminarAsync(int id);

        Task<bool> ExisteAsync(int id);
        Task<string?> CrearConCuentaAsync(
    Medico medico,
    bool crearCuenta,
    string? password);  
        Task<string?> CrearCuentaParaMedicoAsync(
            int medicoId,
            string password);
    }
    
     
}