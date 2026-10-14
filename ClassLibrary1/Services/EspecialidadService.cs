using ClinicaSaludVitails.Core.Interfaces;
using ClinicaSaludVitails.Data;
using ClinicaSaludVitails.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicaSaludVitails.Data.Services
{
    public class EspecialidadService : IEspecialidadService
    {
        private readonly ApplicationDbContext _context;

        public EspecialidadService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Especialidad>> ObtenerTodasAsync()
        {
            return await _context.Especialidades
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Especialidad?> ObtenerPorIdAsync(int id)
        {
            return await _context.Especialidades
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task CrearAsync(Especialidad especialidad)
        {
            _context.Especialidades.Add(especialidad);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Especialidad especialidad)
        {
            _context.Especialidades.Update(especialidad);
            await _context.SaveChangesAsync();
        }


        public async Task<string?> EliminarAsync(int id)
        {
            var especialidad = await _context.Especialidades
                .FirstOrDefaultAsync(e => e.Id == id);

            if (especialidad == null)
                return "No se encontró la especialidad que deseas eliminar.";

            // Comprobar si existen médicos asociados
            var tieneMedicos = await _context.Medicos
                .AnyAsync(m => m.EspecialidadId == id);

            if (tieneMedicos)
            {
                return "No se puede eliminar esta especialidad porque tiene médicos asociados. " +
                       "Primero debes reasignarlos a otra especialidad.";
            }

            _context.Especialidades.Remove(especialidad);
            await _context.SaveChangesAsync();

            return null;
        }


        public async Task<bool> ExisteAsync(int id)
        {
            return await _context.Especialidades
                .AnyAsync(e => e.Id == id);
        }
    }
}