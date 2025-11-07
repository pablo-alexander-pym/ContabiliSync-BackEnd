using System.ComponentModel.DataAnnotations;
using BackEnd.Models;

namespace BackEnd.DTOs
{
    public class CitaCreateDto
    {
        [Required]
        public int ContadorId { get; set; }

        [Required]
        public int ContribuyenteId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TimeSpan Hora { get; set; }

        [Required]
        public EstadoCita Estado { get; set; }

        public string? Notas { get; set; }
    }
}