using ClinicaSaludVitails.Models;

namespace ClinicaSaludVitails.Core.Interfaces
{
    public interface IEspecialidadService
    {
        Task<List<Especialidad>> ObtenerTodasAsync();

        Task<Especialidad?> ObtenerPorIdAsync(int id);

        Task CrearAsync(Especialidad especialidad);

        Task ActualizarAsync(Especialidad especialidad);

        Task<string?> EliminarAsync(int id);

        Task<bool> ExisteAsync(int id);
    }
}