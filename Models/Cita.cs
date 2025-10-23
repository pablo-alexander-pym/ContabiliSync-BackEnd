using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    public class Cita
    {
        public int Id { get; set; }

        [Required]
        public int ContadorId { get; set; }
        public required Usuario Contador { get; set; }

        [Required]
        public int ContribuyenteId { get; set; }
        public required Usuario Contribuyente { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TimeSpan Hora { get; set; }

        [Required]
        public EstadoCita Estado { get; set; }

        public string? Notas { get; set; }
    }

    public enum EstadoCita
    {
        Pendiente,
        Confirmada,
        Cancelada,
        Completada
    }
}