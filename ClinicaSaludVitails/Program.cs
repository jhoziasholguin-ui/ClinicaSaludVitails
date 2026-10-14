using Microsoft.EntityFrameworkCore;
using ClinicaSaludVitails.Data;

using ClinicaSaludVitails.Interfaces;
using ClinicaSaludVitails.Services;
using ClinicaSaludVitails.Core.Interfaces;
using ClinicaSaludVitails.Data.Services;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)));


builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddScoped<ICitaService, CitaService>();
builder.Services.AddScoped<IEspecialidadService, EspecialidadService>();
builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IAccountService, AccountService>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
