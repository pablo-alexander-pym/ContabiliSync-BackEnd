using Microsoft.AspNetCore.Mvc;
using BackEnd.Models;
using BackEnd.Services.Interfaces;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentosController : ControllerBase
    {
        private readonly IDocumentoService _documentoService;

        public DocumentosController(IDocumentoService documentoService)
        {
            _documentoService = documentoService;
        }

        /// <summary>
        /// Obtiene todos los documentos registrados en el sistema
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Documento>>> GetDocumentos()
        {
            var documentos = await _documentoService.GetDocumentosAsync();
            return Ok(documentos);
        }

        /// <summary>
        /// Obtiene un documento específico por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Documento>> GetDocumento(int id)
        {
            var documento = await _documentoService.GetDocumentoByIdAsync(id);
            if (documento == null)
            {
                return NotFound();
            }
            return Ok(documento);
        }

        /// <summary>
        /// Crea un nuevo documento en el sistema con el archivo adjunto
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Documento>> CreateDocumento([FromForm] IFormFile file, [FromForm] int contribuyenteId, [FromForm] string descripcion, [FromForm] TipoDocumento tipo)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No se ha proporcionado ningún archivo");
                }

                var documento = new Documento
                {
                    ContribuyenteId = contribuyenteId,
                    Descripcion = descripcion,
                    Tipo = tipo,
                    FechaCarga = DateTime.UtcNow,
                    Nombre = file.FileName,
                    RutaArchivo = string.Empty,
                    Contribuyente = new Usuario
                    {
                        Id = contribuyenteId,
                        Nombre = "Temporal",
                        Email = "temporal@temp.com",
                        Password = "temp",
                        Tipo = TipoUsuario.Usuario
                    }  // El servicio actualizará con los datos reales
                };

                var documentoCreado = await _documentoService.CreateDocumentoAsync(documento, file);
                return CreatedAtAction(nameof(GetDocumento), new { id = documentoCreado.Id }, documentoCreado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Elimina un documento del sistema y su archivo físico
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumento(int id)
        {
            try
            {
                await _documentoService.DeleteDocumentoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (IOException ex)
            {
                return StatusCode(500, $"Error al eliminar el archivo: {ex.Message}");
            }
        }

        /// <summary>
        /// Descarga un documento específico del sistema
        /// </summary>
        [HttpGet("Download/{id}")]
        public async Task<IActionResult> DownloadDocumento(int id)
        {
            try
            {
                var (fileContents, contentType, fileName) = await _documentoService.DownloadDocumentoAsync(id);
                return File(fileContents, contentType, fileName);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (FileNotFoundException)
            {
                return NotFound("Archivo no encontrado");
            }
        }

        /// <summary>
        /// Obtiene todos los documentos de un contribuyente específico
        /// </summary>
        [HttpGet("Contribuyente/{contribuyenteId}")]
        public async Task<ActionResult<IEnumerable<Documento>>> GetDocumentosPorContribuyente(int contribuyenteId)
        {
            try
            {
                var documentos = await _documentoService.GetDocumentosByContribuyenteAsync(contribuyenteId);
                return Ok(documentos);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}