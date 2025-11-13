namespace BackEnd.Services.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de gestión de contraseñas.
    /// Proporciona métodos para cifrar y verificar contraseñas de forma segura.
    /// </summary>
    public interface IPasswordService
    {
        /// <summary>
        /// Cifra una contraseña en texto plano usando BCrypt.
        /// </summary>
        /// <param name="password">La contraseña en texto plano a cifrar</param>
        /// <returns>La contraseña cifrada (hash)</returns>
        string HashPassword(string password);

        /// <summary>
        /// Verifica si una contraseña en texto plano coincide con su hash almacenado.
        /// </summary>
        /// <param name="password">La contraseña en texto plano</param>
        /// <param name="hashedPassword">El hash de la contraseña almacenado</param>
        /// <returns>True si la contraseña coincide, False en caso contrario</returns>
        bool VerifyPassword(string password, string hashedPassword);
    }
}