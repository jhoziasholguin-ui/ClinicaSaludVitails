using ClinicaSaludVitails.Models;

namespace ClinicaSaludVitails.Core.Interfaces
{
    public interface IAccountService
    {
        Task<Usuario?> AutenticarAsync(
            string correo,
            string password);

        Task<string?> RegistrarAsync(
            Usuario usuario,
            bool permitirRegistroAdmin);
    }
}