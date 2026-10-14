
namespace ClinicaSaludVitails.ViewModels
{
    public class HomeDashboardViewModel
    {
        public int TotalCitas { get; set; }
        public int CitasProgramadas { get; set; }
        public int CitasAtendidas { get; set; }
        public int CitasCanceladas { get; set; }
        public int TotalPacientes { get; set; }
        public int TotalMedicos { get; set; }

        public List<string> Meses { get; set; } = new();
        public List<int> CitasPorMes { get; set; } = new();

        public List<string> Estados { get; set; } = new();
        public List<int> CitasPorEstado { get; set; } = new();
    }
}
