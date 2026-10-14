    using System.ComponentModel.DataAnnotations;

    namespace ClinicaSaludVitails.Models
    {
        public class Especialidad
        {
            [Key]
            public int Id { get; set; }

            [Required(ErrorMessage = "El nombre de la especialidad es obligatorio")]
            [Display(Name = "Especialidad")]
            public string Nombre { get; set; } = string.Empty;
        }
    }