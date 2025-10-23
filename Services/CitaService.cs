using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using BackEnd.Models;
using BackEnd.Services.Interfaces;

namespace BackEnd.Services
{
    public class CitaService : ICitaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUsuarioService _usuarioService;

        public CitaService(ApplicationDbContext context, IUsuarioService usuarioService)
        {
            _context = context;
            _usuarioService = usuarioService;
        }

        public async Task<IEnumerable<Cita>> GetCitasAsync()
        {
            return await _context.Citas
                .Include(c => c.Contador)
                .Include(c => c.Contribuyente)
                .ToListAsync();
        }

        public async Task<Cita?> GetCitaByIdAsync(int id)
        {
            return await _context.Citas
                .Include(c => c.Contador)
                .Include(c => c.Contribuyente)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Cita>> GetCitasByContadorAsync(int contadorId)
        {
            return await _context.Citas
                .Include(c => c.Contador)
                .Include(c => c.Contribuyente)
                .Where(c => c.ContadorId == contadorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cita>> GetCitasByContribuyenteAsync(int contribuyenteId)
        {
            return await _context.Citas
                .Include(c => c.Contador)
                .Include(c => c.Contribuyente)
                .Where(c => c.ContribuyenteId == contribuyenteId)
                .ToListAsync();
        }

        public async Task<Cita> CreateCitaAsync(Cita cita)
        {
            // Validar que existan el contador y el contribuyente
            if (!await _usuarioService.ExisteUsuarioAsync(cita.ContadorId))
                throw new ArgumentException("Contador no válido");

            if (!await _usuarioService.ExisteUsuarioAsync(cita.ContribuyenteId))
                throw new ArgumentException("Contribuyente no válido");

            // Validar disponibilidad
            if (!await ValidarDisponibilidadAsync(cita.Fecha, cita.Hora, cita.ContadorId))
                throw new InvalidOperationException("El contador no está disponible en ese horario");

            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
            return cita;
        }

        public async Task UpdateCitaAsync(int id, Cita cita)
        {
            if (id != cita.Id)
                throw new ArgumentException("ID no coincide con la cita proporcionada");

            _context.Entry(cita).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCitaAsync(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
                throw new KeyNotFoundException("Cita no encontrada");

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidarDisponibilidadAsync(DateTime fecha, TimeSpan hora, int contadorId)
        {
            return !await _context.Citas
                .AnyAsync(c => c.ContadorId == contadorId &&
                              c.Fecha.Date == fecha.Date &&
                              c.Hora == hora &&
                              c.Estado != EstadoCita.Cancelada);
        }
    }
}