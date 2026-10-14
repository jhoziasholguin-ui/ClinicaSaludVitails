using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaSaludVitails.Models
{
    public class Medico
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellidos { get; set; } = string.Empty;
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El CMP es obligatorio")]
        [StringLength(6, MinimumLength = 5, ErrorMessage = "El CMP debe tener entre 5 y 6 dígitos")]
        public string CMP { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seleccione una especialidad")]
        [Display(Name = "Especialidad")]
        public int EspecialidadId { get; set; }

        [ForeignKey("EspecialidadId")]
        public Especialidad? Especialidad { get; set; }

        [Display(Name = "Médico")]
        public string NombreCompleto => $"{Apellidos}, {Nombres}";

        public Usuario? Usuario { get; set; }
    }
}