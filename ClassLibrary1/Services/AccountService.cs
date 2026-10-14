using ClinicaSaludVitails.Core.Interfaces;
using ClinicaSaludVitails.Models;
using ClinicaSaludVitails.Core.Util;
using Microsoft.EntityFrameworkCore;

namespace ClinicaSaludVitails.Data.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> AutenticarAsync(
            string correo,
            string password)
        {
            string passwordHash = Encrypt.GetSHA256(password);

            return await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.Correo == correo &&
                    u.Password == passwordHash);
        }

        public async Task<string?> RegistrarAsync(
            Usuario usuario,
            bool permitirRegistroAdmin)
        {
            // Impedir el registro público de administradores
            // cuando la configuración lo deshabilita.
            if (usuario.Rol == "Administrador" &&
                !permitirRegistroAdmin)
            {
                return "El registro público de administradores está deshabilitado por seguridad.";
            }

            bool correoExiste = await _context.Usuarios
                .AnyAsync(u => u.Correo == usuario.Correo);

            if (correoExiste)
            {
                return "Este correo electrónico ya está registrado.";
            }

            usuario.Password = Encrypt.GetSHA256(usuario.Password);

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return null;
        }
    }
}