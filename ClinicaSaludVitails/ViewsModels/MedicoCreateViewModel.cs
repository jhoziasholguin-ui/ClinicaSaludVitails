
using System.ComponentModel.DataAnnotations;

namespace ClinicaSaludVitails.ViewModels
{
    public class MedicoCreateViewModel : IValidatableObject
    {
        [Required]
        public string Nombres { get; set; } = string.Empty;

        [Required]
        public string Apellidos { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [Required]
        [StringLength(6, MinimumLength = 5)]
        public string CMP { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Especialidad")]
        public int EspecialidadId { get; set; }

        [Display(Name = "Crear cuenta de acceso")]
        public bool CrearCuenta { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (CrearCuenta)
            {
                if (string.IsNullOrWhiteSpace(Password))
                {
                    yield return new ValidationResult(
                        "La contraseña es obligatoria para crear la cuenta.",
                        new[] { nameof(Password) });
                }
                else if (Password.Length < 6 ||
                         !Password.Any(char.IsUpper) ||
                         !Password.Any(char.IsLower) ||
                         !Password.Any(char.IsDigit))
                {
                    yield return new ValidationResult(
                        "La contraseña debe tener al menos 6 caracteres, " +
                        "una mayúscula, una minúscula y un número.",
                        new[] { nameof(Password) });
                }
            }
        }
    }
}
