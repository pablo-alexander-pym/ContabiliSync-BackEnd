using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public TipoUsuario Tipo { get; set; }

        public string? Telefono { get; set; }
        
        // Solo para contadores
        public string? Especialidad { get; set; }
        public string? NumeroLicencia { get; set; }
    }

    public enum TipoUsuario
    {
        Contador,
        Contribuyente
    }
}