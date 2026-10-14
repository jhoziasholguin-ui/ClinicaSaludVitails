using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore; // IMPORTANTE: Agrega este using para los índices

namespace ClinicaSaludVitails.Models
{
    [Index(nameof(DNI), IsUnique = true)] // <--- ESTE ES EL CANDADO EN MYSQL
    public class Paciente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener exactamente 8 dígitos")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El DNI solo debe contener números")]
        public string DNI { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; } = string.Empty;

        [Display(Name = "Paciente")]
        public string NombreCompleto => $"{Apellidos}, {Nombres}";
    }
}