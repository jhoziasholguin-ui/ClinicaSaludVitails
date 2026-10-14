using ClinicaSaludVitails.Core.Interfaces;
using ClinicaSaludVitails.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicaSaludVitails.Data.Services
{
    public class MedicoService : IMedicoService
    {
        private readonly ApplicationDbContext _context;

        public MedicoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Medico>> ObtenerTodosAsync()
        {
            return await _context.Medicos
                .Include(m => m.Especialidad)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Medico?> ObtenerPorIdAsync(int id)
        {
            return await _context.Medicos
                .Include(m => m.Especialidad)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Especialidad>> ObtenerEspecialidadesAsync()
        {
            return await _context.Especialidades
                .AsNoTracking()
                .OrderBy(e => e.Nombre)
                .ToListAsync();
        }

        public async Task CrearAsync(Medico medico)
        {
            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();
        }


        public async Task ActualizarAsync(Medico medico)
        {
            var existente = await _context.Medicos
                .FirstOrDefaultAsync(m => m.Id == medico.Id);

            if (existente == null)
                throw new InvalidOperationException(
                    "No se encontró el médico que deseas actualizar.");

            existente.Nombres = medico.Nombres;
            existente.Apellidos = medico.Apellidos;
            existente.Correo = medico.Correo;
            existente.CMP = medico.CMP;
            existente.EspecialidadId = medico.EspecialidadId;

            await _context.SaveChangesAsync();
        }
        public async Task<string?> CrearCuentaParaMedicoAsync(
    int medicoId,
    string password)
        {
            var medico = await _context.Medicos
                .FirstOrDefaultAsync(m => m.Id == medicoId);

            if (medico == null)
                return "No se encontró el médico.";

            // Comprobar si ya tiene una cuenta vinculada
            var tieneCuenta = await _context.Usuarios
                .AnyAsync(u => u.MedicoId == medicoId);

            if (tieneCuenta)
                return "Este médico ya tiene una cuenta de acceso.";

            // Comprobar que el correo no esté registrado
            var correoExiste = await _context.Usuarios
                .AnyAsync(u => u.Correo == medico.Correo);

            if (correoExiste)
                return "Ya existe una cuenta con el correo de este médico.";

            if (string.IsNullOrWhiteSpace(password) ||
                password.Length < 6 ||
                !password.Any(char.IsUpper) ||
                !password.Any(char.IsLower) ||
                !password.Any(char.IsDigit))
            {
                return "La contraseña debe tener al menos 6 caracteres, una mayúscula, una minúscula y un número.";
            }

            var usuario = new Usuario
            {
                Correo = medico.Correo,
                Password = ClinicaSaludVitails.Core.Util.Encrypt
                    .GetSHA256(password),
                Rol = "Medico",
                MedicoId = medico.Id
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return null;
        }


        public async Task<string?> EliminarAsync(int id)
        {
            var medico = await _context.Medicos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medico == null)
                return "No se encontró el médico que deseas eliminar.";

            // Comprobar si el médico tiene citas registradas
            var tieneCitas = await _context.Citas
                .AnyAsync(c => c.MedicoId == id);

            if (tieneCitas)
            {
                return "No se puede eliminar este médico porque tiene citas registradas. " +
                       "Para conservar el historial, se recomienda desactivarlo.";
            }

            // Comprobar si el médico tiene una cuenta vinculada
            var tieneUsuario = await _context.Usuarios
                .AnyAsync(u => u.MedicoId == id);

            if (tieneUsuario)
            {
                return "No se puede eliminar este médico porque tiene una cuenta de usuario vinculada.";
            }

            _context.Medicos.Remove(medico);
            await _context.SaveChangesAsync();

            return null;
        }


        public async Task<bool> ExisteAsync(int id)
        {
            return await _context.Medicos
                .AnyAsync(m => m.Id == id);
        }


        public async Task<string?> CrearConCuentaAsync(
            Medico medico,
            bool crearCuenta,
            string? password)
        {
            if (await _context.Medicos.AnyAsync(
                m => m.Correo == medico.Correo))
            {
                return "Ya existe un médico con ese correo.";
            }

            if (await _context.Medicos.AnyAsync(
                m => m.CMP == medico.CMP))
            {
                return "Ya existe un médico con ese CMP.";
            }

            if (crearCuenta)
            {
                if (string.IsNullOrWhiteSpace(password))
                    return "Debes indicar una contraseña.";

                if (await _context.Usuarios.AnyAsync(
                    u => u.Correo == medico.Correo))
                {
                    return "Ya existe una cuenta con ese correo.";
                }
            }

            await using var transaccion =
                await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Medicos.Add(medico);
                await _context.SaveChangesAsync();

                if (crearCuenta)
                {
                    var usuario = new Usuario
                    {
                        Correo = medico.Correo,
                        Password = ClinicaSaludVitails.Core.Util
                            .Encrypt.GetSHA256(password!),
                        Rol = "Medico",
                        MedicoId = medico.Id
                    };

                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();
                }

                await transaccion.CommitAsync();
                return null;
            }
            catch
            {
                await transaccion.RollbackAsync();
                throw;
            }
        }

    }
}