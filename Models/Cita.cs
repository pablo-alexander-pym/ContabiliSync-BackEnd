using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    /// <summary>
    /// Representa una cita programada entre un contribuyente y un contador en el sistema.
    /// Esta entidad gestiona las citas de asesoría contable y tributaria.
    /// </summary>
    public class Cita
    {
        /// <summary>
        /// Identificador único de la cita en la base de datos.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador del contador asignado a la cita.
        /// Referencia a la tabla Usuario donde Tipo = Contador.
        /// </summary>
        [Required]
        public int ContadorId { get; set; }

        /// <summary>
        /// Navegación a la entidad Usuario que representa al contador.
        /// Permite acceder a la información completa del contador profesional.
        /// </summary>
        public Usuario? Contador { get; set; }

        /// <summary>
        /// Identificador del contribuyente que solicita la cita.
        /// Referencia a la tabla Usuario donde Tipo = Usuario.
        /// </summary>
        [Required]
        public int ContribuyenteId { get; set; }

        /// <summary>
        /// Navegación a la entidad Usuario que representa al contribuyente.
        /// Permite acceder a la información completa del solicitante de la cita.
        /// </summary>
        public Usuario? Contribuyente { get; set; }

        /// <summary>
        /// Fecha programada para la cita.
        /// Campo obligatorio que debe estar en el futuro al momento de creación.
        /// </summary>
        [Required]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Hora específica de la cita en formato TimeSpan.
        /// Representa la hora del día sin componente de fecha.
        /// </summary>
        [Required]
        public TimeSpan Hora { get; set; }

        /// <summary>
        /// Estado actual de la cita en el flujo de proceso.
        /// Controla el ciclo de vida desde la creación hasta la finalización.
        /// </summary>
        [Required]
        public EstadoCita Estado { get; set; }

        /// <summary>
        /// Notas adicionales sobre la cita.
        /// Campo opcional para comentarios, preparación requerida, o temas específicos a tratar.
        /// </summary>
        public string? Notas { get; set; }
    }

    /// <summary>
    /// Enumeración que define los posibles estados de una cita en el sistema.
    /// </summary>
    public enum EstadoCita
    {
        /// <summary>
        /// Cita creada pero pendiente de confirmación por parte del contador.
        /// </summary>
        Pendiente,

        /// <summary>
        /// Cita confirmada por el contador y programada definitivamente.
        /// </summary>
        Confirmada,

        /// <summary>
        /// Cita cancelada por cualquiera de las partes antes de su realización.
        /// </summary>
        Cancelada,

        /// <summary>
        /// Cita realizada exitosamente y finalizada.
        /// </summary>
        Completada
    }
}