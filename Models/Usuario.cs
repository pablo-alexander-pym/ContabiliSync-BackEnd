using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    /// <summary>
    /// Representa un usuario en el sistema ContabiliSync.
    /// Esta entidad almacena la información básica de usuarios, contadores y administradores.
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Identificador único del usuario en la base de datos.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre completo del usuario.
        /// Campo obligatorio con longitud máxima de 100 caracteres.
        /// </summary>
        [Required]
        [StringLength(100)]
        public required string Nombre { get; set; }

        /// <summary>
        /// Dirección de correo electrónico del usuario.
        /// Campo obligatorio que debe tener formato de email válido.
        /// Se utiliza para autenticación y comunicaciones.
        /// </summary>
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// Contraseña del usuario para autenticación.
        /// Campo obligatorio que debe almacenarse de forma segura (hash).
        /// </summary>
        [Required]
        public required string Password { get; set; }

        /// <summary>
        /// Tipo de usuario que determina los permisos y funcionalidades disponibles.
        /// Define si es un usuario regular, contador profesional o administrador del sistema.
        /// </summary>
        [Required]
        public TipoUsuario Tipo { get; set; }

        /// <summary>
        /// Número de teléfono del usuario.
        /// Campo opcional para contacto adicional.
        /// </summary>
        public string? Telefono { get; set; }

        /// <summary>
        /// Especialidad profesional del contador.
        /// Solo aplica para usuarios de tipo Contador.
        /// Ejemplo: "Tributario", "Financiero", "Laboral", etc.
        /// </summary>
        public string? Especialidad { get; set; }

        /// <summary>
        /// Número de licencia profesional del contador.
        /// Solo aplica para usuarios de tipo Contador.
        /// Identifica la credencial profesional oficial.
        /// </summary>
        public string? NumeroLicencia { get; set; }
    }

    /// <summary>
    /// Enumeración que define los tipos de usuario en el sistema.
    /// </summary>
    public enum TipoUsuario
    {
        /// <summary>
        /// 0. Usuario regular (contribuyente) que puede gestionar sus documentos y citas.
        /// </summary>
        Usuario,

        /// <summary>
        /// 1. Contador profesional que puede atender citas y revisar documentos de contribuyentes.
        /// </summary>
        Contador,

        /// <summary>
        /// 2. Administrador del sistema con permisos completos de gestión.
        /// </summary>
        Administrador
    }
}