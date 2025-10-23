using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    public class Documento
    {
        public int Id { get; set; }

        [Required]
        public int ContribuyenteId { get; set; }
        public required Usuario Contribuyente { get; set; }

        [Required]
        public required string Nombre { get; set; }

        [Required]
        public required string RutaArchivo { get; set; }

        [Required]
        public DateTime FechaCarga { get; set; }

        public TipoDocumento Tipo { get; set; }

        public string? Descripcion { get; set; }
    }

    public enum TipoDocumento
    {
        CertificadoLaboral,
        CertificadoBancario,
        FacturasGastos,
        DeclaracionAnterior,
        Otro
    }
}