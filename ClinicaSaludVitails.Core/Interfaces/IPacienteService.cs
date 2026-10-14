using ClinicaSaludVitails.Models;

namespace ClinicaSaludVitails.Core.Interfaces
{
    public interface IPacienteService
    {
        Task<List<Paciente>> ObtenerTodosAsync();

        Task<Paciente?> ObtenerPorIdAsync(int id);

        Task<bool> ExisteDniAsync(string dni, int? excluirPacienteId = null);

        Task CrearAsync(Paciente paciente);

        Task ActualizarAsync(Paciente paciente);

        Task EliminarAsync(int id);

        Task<bool> ExisteAsync(int id);
    }
}