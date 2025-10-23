using BackEnd.Models;

namespace BackEnd.Services.Interfaces
{
    public interface ICitaService
    {
        Task<IEnumerable<Cita>> GetCitasAsync();
        Task<Cita?> GetCitaByIdAsync(int id);
        Task<IEnumerable<Cita>> GetCitasByContadorAsync(int contadorId);
        Task<IEnumerable<Cita>> GetCitasByContribuyenteAsync(int contribuyenteId);
        Task<Cita> CreateCitaAsync(Cita cita);
        Task UpdateCitaAsync(int id, Cita cita);
        Task DeleteCitaAsync(int id);
        Task<bool> ValidarDisponibilidadAsync(DateTime fecha, TimeSpan hora, int contadorId);
    }
}