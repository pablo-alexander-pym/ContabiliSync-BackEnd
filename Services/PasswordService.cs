using BCrypt.Net;
using BackEnd.Services.Interfaces;

namespace BackEnd.Services
{
    /// <summary>
    /// Servicio para la gestión segura de contraseñas usando BCrypt.
    /// Implementa el cifrado y verificación de contraseñas con salt automático.
    /// </summary>
    public class PasswordService : IPasswordService
    {
        /// <summary>
        /// Cifra una contraseña en texto plano usando BCrypt con salt automático.
        /// Utiliza un trabajo factor de 12 para proporcionar un balance entre seguridad y rendimiento.
        /// </summary>
        /// <param name="password">La contraseña en texto plano a cifrar</param>
        /// <returns>La contraseña cifrada (hash) que incluye el salt</returns>
        /// <exception cref="ArgumentException">Se lanza si la contraseña es null o vacía</exception>
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("La contraseña no puede ser null o vacía", nameof(password));
            }

            // Utilizamos workFactor 12 que es un buen balance entre seguridad y rendimiento
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }

        /// <summary>
        /// Verifica si una contraseña en texto plano coincide con su hash almacenado.
        /// </summary>
        /// <param name="password">La contraseña en texto plano a verificar</param>
        /// <param name="hashedPassword">El hash de la contraseña almacenado en la base de datos</param>
        /// <returns>True si la contraseña coincide con el hash, False en caso contrario</returns>
        /// <exception cref="ArgumentException">Se lanza si algún parámetro es null o vacío</exception>
        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("La contraseña no puede ser null o vacía", nameof(password));
            }

            if (string.IsNullOrWhiteSpace(hashedPassword))
            {
                throw new ArgumentException("El hash de la contraseña no puede ser null o vacío", nameof(hashedPassword));
            }

            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (Exception)
            {
                // Si hay algún error en la verificación (hash malformado, etc.), devolvemos false
                return false;
            }
        }
    }
}