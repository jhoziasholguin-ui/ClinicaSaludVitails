using ClinicaSaludVitails.Core.Interfaces;
using ClinicaSaludVitails.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicaSaludVitails.Data.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly ApplicationDbContext _context;

        public PacienteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Paciente>> ObtenerTodosAsync()
        {
            return await _context.Pacientes
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Paciente?> ObtenerPorIdAsync(int id)
        {
            return await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> ExisteDniAsync(
            string dni,
            int? excluirPacienteId = null)
        {
            return await _context.Pacientes.AnyAsync(p =>
                p.DNI == dni &&
                (!excluirPacienteId.HasValue ||
                 p.Id != excluirPacienteId.Value));
        }

        public async Task CrearAsync(Paciente paciente)
        {
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Paciente paciente)
        {
            _context.Pacientes.Update(paciente);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);

            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _context.Pacientes
                .AnyAsync(p => p.Id == id);
        }
    }
}