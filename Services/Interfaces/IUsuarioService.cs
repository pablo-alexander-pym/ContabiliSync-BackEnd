using BackEnd.Models;

namespace BackEnd.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetUsuariosAsync();
        Task<Usuario?> GetUsuarioByIdAsync(int id);
        Task<Usuario> CreateUsuarioAsync(Usuario usuario);
        Task UpdateUsuarioAsync(int id, Usuario usuario);
        Task DeleteUsuarioAsync(int id);
        Task<IEnumerable<Usuario>> GetContadoresAsync();
        Task<IEnumerable<Usuario>> GetUsuariosByTipoAsync(TipoUsuario tipo);
        Task<bool> ExisteUsuarioAsync(int id);
    }
}