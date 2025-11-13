using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using BackEnd.Models;
using BackEnd.Services.Interfaces;

namespace BackEnd.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordService _passwordService;

        public UsuarioService(ApplicationDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario?> GetUsuarioByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> CreateUsuarioAsync(Usuario usuario)
        {
            // Validar que el email no esté en uso
            var existeEmail = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
            if (existeEmail)
            {
                throw new ArgumentException("El email ya está en uso");
            }

            // Cifrar la contraseña antes de guardar
            usuario.Password = _passwordService.HashPassword(usuario.Password);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task UpdateUsuarioAsync(int id, Usuario usuario)
        {
            if (id != usuario.Id)
                throw new ArgumentException("ID no coincide con el usuario proporcionado");

            var usuarioExistente = await _context.Usuarios.FindAsync(id);
            if (usuarioExistente == null)
                throw new KeyNotFoundException("Usuario no encontrado");

            // Validar que el email no esté en uso por otro usuario
            var existeEmail = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email && u.Id != id);
            if (existeEmail)
            {
                throw new ArgumentException("El email ya está en uso");
            }

            // Actualizar campos
            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Email = usuario.Email;
            usuarioExistente.Tipo = usuario.Tipo;
            usuarioExistente.Telefono = usuario.Telefono;
            usuarioExistente.Especialidad = usuario.Especialidad;
            usuarioExistente.NumeroLicencia = usuario.NumeroLicencia;

            // Solo cifrar la contraseña si es diferente (no está ya cifrada)
            if (!string.IsNullOrWhiteSpace(usuario.Password) &&
                usuario.Password != usuarioExistente.Password)
            {
                // Si la nueva contraseña no parece estar cifrada (no empieza con $2a$), la ciframos
                if (!usuario.Password.StartsWith("$2a$") && !usuario.Password.StartsWith("$2b$") && !usuario.Password.StartsWith("$2y$"))
                {
                    usuarioExistente.Password = _passwordService.HashPassword(usuario.Password);
                }
                else
                {
                    usuarioExistente.Password = usuario.Password;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                throw new KeyNotFoundException("Usuario no encontrado");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Usuario>> GetContadoresAsync()
        {
            return await GetUsuariosByTipoAsync(TipoUsuario.Contador);
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosByTipoAsync(TipoUsuario tipo)
        {
            return await _context.Usuarios
                .Where(u => u.Tipo == tipo)
                .ToListAsync();
        }

        public async Task<bool> ExisteUsuarioAsync(int id)
        {
            return await _context.Usuarios.AnyAsync(u => u.Id == id);
        }

        public async Task<Usuario?> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null)
            {
                return null;
            }

            // Verificar la contraseña
            if (!_passwordService.VerifyPassword(password, usuario.Password))
            {
                return null;
            }

            return usuario;
        }

        public async Task<bool> ChangePasswordAsync(int id, string currentPassword, string newPassword)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return false;
            }

            // Verificar la contraseña actual
            if (!_passwordService.VerifyPassword(currentPassword, usuario.Password))
            {
                return false;
            }

            // Cifrar y guardar la nueva contraseña
            usuario.Password = _passwordService.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}