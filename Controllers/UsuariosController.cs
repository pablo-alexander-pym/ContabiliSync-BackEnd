using Microsoft.AspNetCore.Mvc;
using BackEnd.Models;
using BackEnd.Services.Interfaces;
using BackEnd.DTOs;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Obtiene todos los usuarios registrados en el sistema
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _usuarioService.GetUsuariosAsync();
            return Ok(usuarios);
        }

        /// <summary>
        /// Obtiene un usuario específico por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        /// <summary>
        /// Obtiene la lista de todos los contadores registrados
        /// </summary>
        [HttpGet("Contadores")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetContadores()
        {
            var contadores = await _usuarioService.GetContadoresAsync();
            return Ok(contadores);
        }

        /// <summary>
        /// Obtiene la lista de usuarios por tipo (Usuario, Contador, Administrador)
        /// </summary>
        [HttpGet("PorTipo/{tipo}")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuariosPorTipo(TipoUsuario tipo)
        {
            var usuarios = await _usuarioService.GetUsuariosByTipoAsync(tipo);
            return Ok(usuarios);
        }

        /// <summary>
        /// Crea un nuevo usuario en el sistema
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
        {
            try
            {
                var nuevoUsuario = await _usuarioService.CreateUsuarioAsync(usuario);
                return CreatedAtAction(nameof(GetUsuario), new { id = nuevoUsuario.Id }, nuevoUsuario);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Actualiza la información de un usuario existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, Usuario usuario)
        {
            try
            {
                await _usuarioService.UpdateUsuarioAsync(id, usuario);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Elimina un usuario del sistema
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                await _usuarioService.DeleteUsuarioAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Autentica un usuario con email y contraseña
        /// </summary>
        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            try
            {
                var usuario = await _usuarioService.AuthenticateAsync(loginDto.Email, loginDto.Password);

                if (usuario == null)
                {
                    return Unauthorized(new { message = "Email o contraseña incorrectos" });
                }

                var response = new AuthResponseDto
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    Tipo = usuario.Tipo.ToString(),
                    Message = "Autenticación exitosa"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error en la autenticación", error = ex.Message });
            }
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema
        /// </summary>
        [HttpPost("Registro")]
        public async Task<ActionResult<AuthResponseDto>> Registro(RegisterDto registerDto)
        {
            try
            {
                var nuevoUsuario = new Usuario
                {
                    Nombre = registerDto.Nombre,
                    Email = registerDto.Email,
                    Password = registerDto.Password,
                    Telefono = registerDto.Telefono,
                    Especialidad = registerDto.Especialidad,
                    NumeroLicencia = registerDto.NumeroLicencia,
                    Tipo = (TipoUsuario)registerDto.Tipo
                };

                var usuarioCreado = await _usuarioService.CreateUsuarioAsync(nuevoUsuario);

                var response = new AuthResponseDto
                {
                    Id = usuarioCreado.Id,
                    Nombre = usuarioCreado.Nombre,
                    Email = usuarioCreado.Email,
                    Tipo = usuarioCreado.Tipo.ToString(),
                    Message = "Usuario registrado exitosamente"
                };

                return CreatedAtAction(nameof(GetUsuario), new { id = usuarioCreado.Id }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al registrar usuario", error = ex.Message });
            }
        }

        /// <summary>
        /// Cambia la contraseña de un usuario
        /// </summary>
        [HttpPost("{id}/CambiarPassword")]
        public async Task<IActionResult> CambiarPassword(int id, ChangePasswordDto changePasswordDto)
        {
            try
            {
                var resultado = await _usuarioService.ChangePasswordAsync(id, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

                if (!resultado)
                {
                    return BadRequest(new { message = "Contraseña actual incorrecta o usuario no encontrado" });
                }

                return Ok(new { message = "Contraseña cambiada exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al cambiar la contraseña", error = ex.Message });
            }
        }
    }
}