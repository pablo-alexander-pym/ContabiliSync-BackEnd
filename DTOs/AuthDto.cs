using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOs
{
    /// <summary>
    /// DTO para el proceso de login/autenticación de usuarios
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Email del usuario para autenticación
        /// </summary>
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// Contraseña del usuario en texto plano
        /// </summary>
        [Required]
        public required string Password { get; set; }
    }

    /// <summary>
    /// DTO para cambiar la contraseña de un usuario
    /// </summary>
    public class ChangePasswordDto
    {
        /// <summary>
        /// Contraseña actual del usuario
        /// </summary>
        [Required]
        public required string CurrentPassword { get; set; }

        /// <summary>
        /// Nueva contraseña que el usuario desea establecer
        /// </summary>
        [Required]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public required string NewPassword { get; set; }
    }

    /// <summary>
    /// DTO para registrar un nuevo usuario
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// Nombre completo del usuario
        /// </summary>
        [Required]
        public required string Nombre { get; set; }

        /// <summary>
        /// Email del usuario
        /// </summary>
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// Contraseña del usuario
        /// </summary>
        [Required]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public required string Password { get; set; }

        /// <summary>
        /// Teléfono del usuario (opcional)
        /// </summary>
        public string? Telefono { get; set; }

        /// <summary>
        /// Especialidad del usuario (opcional, para contadores)
        /// </summary>
        public string? Especialidad { get; set; }

        /// <summary>
        /// Número de licencia (opcional, para contadores)
        /// </summary>
        public string? NumeroLicencia { get; set; }

        /// <summary>
        /// Tipo de usuario (por defecto Usuario)
        /// </summary>
        public int Tipo { get; set; } = 0; // Usuario por defecto
    }

    /// <summary>
    /// DTO para la respuesta de autenticación exitosa
    /// </summary>
    public class AuthResponseDto
    {
        /// <summary>
        /// ID del usuario autenticado
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre completo del usuario
        /// </summary>
        public required string Nombre { get; set; }

        /// <summary>
        /// Email del usuario
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Tipo de usuario (Usuario, Contador, Administrador)
        /// </summary>
        public required string Tipo { get; set; }

        /// <summary>
        /// Mensaje de éxito
        /// </summary>
        public required string Message { get; set; }
    }
}