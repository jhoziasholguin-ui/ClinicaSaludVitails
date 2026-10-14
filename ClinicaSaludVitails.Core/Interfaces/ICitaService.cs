
using ClinicaSaludVitails.Models;

namespace ClinicaSaludVitails.Interfaces
{
    public interface ICitaService
    {
        Task<List<Cita>> ObtenerTodasAsync();

        Task<Cita?> ObtenerPorIdAsync(int id);

        Task<bool> MedicoOcupadoAsync(
            int medicoId,
            DateTime fecha,
            TimeSpan hora,
            int? excluirCitaId = null);

        Task<bool> CrearAsync(Cita cita);

        Task<bool> ActualizarAsync(Cita cita);

        Task<bool> EliminarAsync(int id);

        Task<List<Cita>> ObtenerAgendaPorCorreoAsync(string correo);

        Task<bool> ExisteAsync(int id);


        Task<List<Medico>> ObtenerMedicosAsync();

        Task<List<Paciente>> ObtenerPacientesAsync();

    }
}
