using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    /// <summary>
    /// Representa un documento cargado por un contribuyente en el sistema.
    /// Esta entidad gestiona los archivos necesarios para el proceso de declaración tributaria.
    /// </summary>
    public class Documento
    {
        /// <summary>
        /// Identificador único del documento en la base de datos.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador del contribuyente propietario del documento.
        /// Referencia a la tabla Usuario donde Tipo = Usuario.
        /// </summary>
        [Required]
        public int ContribuyenteId { get; set; }

        /// <summary>
        /// Navegación a la entidad Usuario que representa al contribuyente.
        /// Permite acceder a la información completa del propietario del documento.
        /// </summary>
        public required Usuario Contribuyente { get; set; }

        /// <summary>
        /// Nombre descriptivo del documento.
        /// Campo obligatorio que facilita la identificación del archivo por parte del usuario.
        /// </summary>
        [Required]
        public required string Nombre { get; set; }

        /// <summary>
        /// Ruta física o URL donde se almacena el archivo en el sistema.
        /// Campo obligatorio que permite la recuperación del documento almacenado.
        /// </summary>
        [Required]
        public required string RutaArchivo { get; set; }

        /// <summary>
        /// Fecha y hora en que se cargó el documento al sistema.
        /// Se establece automáticamente al momento de la creación.
        /// </summary>
        [Required]
        public DateTime FechaCarga { get; set; }

        /// <summary>
        /// Clasificación del documento según su propósito tributario.
        /// Ayuda en la organización y proceso de revisión de la documentación.
        /// </summary>
        public TipoDocumento Tipo { get; set; }

        /// <summary>
        /// Descripción adicional del documento.
        /// Campo opcional para detalles específicos, período fiscal, o instrucciones especiales.
        /// </summary>
        public string? Descripcion { get; set; }
    }

    /// <summary>
    /// Enumeración que define los tipos de documentos tributarios soportados por el sistema.
    /// </summary>
    public enum TipoDocumento
    {
        /// <summary>
        /// Certificado de ingresos y retenciones laborales expedido por el empleador.
        /// </summary>
        CertificadoLaboral,

        /// <summary>
        /// Certificado de ingresos financieros expedido por entidades bancarias.
        /// </summary>
        CertificadoBancario,

        /// <summary>
        /// Facturas de gastos deducibles para la declaración de renta.
        /// </summary>
        FacturasGastos,

        /// <summary>
        /// Copia de la declaración de renta del año anterior.
        /// </summary>
        DeclaracionAnterior,

        /// <summary>
        /// Cualquier otro tipo de documento relevante para el proceso tributario.
        /// </summary>
        Otro
    }
}