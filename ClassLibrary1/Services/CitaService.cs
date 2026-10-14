
using ClinicaSaludVitails.Data;
using ClinicaSaludVitails.Interfaces;
using ClinicaSaludVitails.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicaSaludVitails.Services
{
    public class CitaService : ICitaService
    {
        private readonly ApplicationDbContext _context;

        public CitaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cita>> ObtenerTodasAsync()
        {
            return await _context.Citas
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .ToListAsync();
        }

        public async Task<Cita?> ObtenerPorIdAsync(int id)
        {
            return await _context.Citas
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(c => c.Id == id);
        }


        public async Task<bool> MedicoOcupadoAsync(
            int medicoId,
            DateTime fecha,
            TimeSpan hora,
            int? excluirCitaId = null)
        {
            return await _context.Citas.AnyAsync(c =>
                c.MedicoId == medicoId &&
                c.Fecha.Date == fecha.Date &&
                c.Hora == hora &&
                c.Estado == "Programada" &&
                (!excluirCitaId.HasValue ||
                 c.Id != excluirCitaId.Value));
        }



        public async Task<bool> CrearAsync(Cita cita)
        {
            if (cita.Estado == "Programada")
            {
                bool horarioOcupado = await MedicoOcupadoAsync(
                    cita.MedicoId,
                    cita.Fecha,
                    cita.Hora);

                if (horarioOcupado)
                {
                    return false;
                }
            }

            _context.Citas.Add(cita);

            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<bool> ActualizarAsync(Cita cita)
        {
            var existente = await _context.Citas
                .FirstOrDefaultAsync(c => c.Id == cita.Id);

            if (existente == null)
            {
                return false;
            }

            if (cita.Estado == "Programada")
            {
                bool horarioOcupado = await MedicoOcupadoAsync(
                    cita.MedicoId,
                    cita.Fecha,
                    cita.Hora,
                    cita.Id);

                if (horarioOcupado)
                {
                    return false;
                }
            }

            existente.PacienteId = cita.PacienteId;
            existente.MedicoId = cita.MedicoId;
            existente.Fecha = cita.Fecha;
            existente.Hora = cita.Hora;
            existente.Estado = cita.Estado;

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> EliminarAsync(int id)
        {
            var cita = await _context.Citas.FindAsync(id);

            if (cita == null)
                return false;

            _context.Citas.Remove(cita);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Cita>> ObtenerAgendaPorCorreoAsync(
            string correo)
        {
            return await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .Where(c => c.Medico.Correo == correo)
                .OrderBy(c => c.Fecha)
                .ThenBy(c => c.Hora)
                .ToListAsync();
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _context.Citas
                .AnyAsync(c => c.Id == id);
        }




        public async Task<List<Medico>> ObtenerMedicosAsync()
        {
            return await _context.Medicos.ToListAsync();
        }

        public async Task<List<Paciente>> ObtenerPacientesAsync()
        {
            return await _context.Pacientes.ToListAsync();
        }

    }
}
