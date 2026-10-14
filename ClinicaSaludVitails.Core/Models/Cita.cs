using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaSaludVitails.Models
{
    public class Cita
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Seleccione un paciente")]
        [Display(Name = "Paciente")]
        public int PacienteId { get; set; }

        [ForeignKey("PacienteId")]
        public Paciente? Paciente { get; set; }

        [Required(ErrorMessage = "Seleccione un médico")]
        [Display(Name = "Médico")]
        public int MedicoId { get; set; }

        [ForeignKey("MedicoId")]
        public Medico? Medico { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de la Cita")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La hora es obligatoria")]
        [Display(Name = "Hora de la Cita")]
        public TimeSpan Hora { get; set; }  // Ejemplo: "09:00", "10:00"

        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Programada"; // Programada, Atendida, Cancelada
    }
}