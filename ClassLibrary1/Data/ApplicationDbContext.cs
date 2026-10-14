using Microsoft.EntityFrameworkCore;
using ClinicaSaludVitails.Models;

namespace ClinicaSaludVitails.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Medico)
                .WithOne(m => m.Usuario)
                .HasForeignKey<Usuario>(u => u.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

   
    }