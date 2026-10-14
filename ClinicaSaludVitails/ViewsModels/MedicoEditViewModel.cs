
using System.ComponentModel.DataAnnotations;

namespace ClinicaSaludVitails.ViewModels
{
    public class MedicoEditViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingresa un correo válido")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El CMP es obligatorio")]
        [StringLength(6, MinimumLength = 5)]
        public string CMP { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecciona una especialidad")]
        [Display(Name = "Especialidad")]
        public int EspecialidadId { get; set; }

        public bool TieneCuenta { get; set; }

        [Display(Name = "Crear cuenta de acceso")]
        public bool CrearCuenta { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string? Password { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (CrearCuenta && !TieneCuenta)
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
                        "La contraseña debe tener al menos 6 caracteres, una mayúscula, una minúscula y un número.",
                        new[] { nameof(Password) });
                }
            }
        }
    }
}
