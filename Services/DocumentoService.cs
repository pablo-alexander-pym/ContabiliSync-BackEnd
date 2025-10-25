using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using BackEnd.Models;
using BackEnd.Services.Interfaces;

namespace BackEnd.Services
{
    public class DocumentoService : IDocumentoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IUsuarioService _usuarioService;

        public DocumentoService(ApplicationDbContext context, IWebHostEnvironment environment, IUsuarioService usuarioService)
        {
            _context = context;
            _environment = environment;
            _usuarioService = usuarioService;
        }

        public async Task<IEnumerable<Documento>> GetDocumentosAsync()
        {
            return await _context.Documentos
                .Include(d => d.Contribuyente)
                .ToListAsync();
        }

        public async Task<Documento?> GetDocumentoByIdAsync(int id)
        {
            return await _context.Documentos
                .Include(d => d.Contribuyente)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Documento>> GetDocumentosByContribuyenteAsync(int contribuyenteId)
        {
            return await _context.Documentos
                .Include(d => d.Contribuyente)
                .Where(d => d.ContribuyenteId == contribuyenteId)
                .ToListAsync();
        }

        public async Task<Documento> CreateDocumentoAsync(Documento documento, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No se ha proporcionado ningún archivo");

            var usuario = await _usuarioService.GetUsuarioByIdAsync(documento.ContribuyenteId);
            if (usuario == null || usuario.Tipo != TipoUsuario.Usuario)
                throw new ArgumentException("Usuario no válido");

            // Crear directorio para documentos si no existe
            var uploadsFolder = Path.Combine(_environment.ContentRootPath, "Uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Generar nombre único para el archivo
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Guardar el archivo
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            documento.RutaArchivo = uniqueFileName;
            documento.FechaCarga = DateTime.UtcNow;
            documento.Nombre = file.FileName;
            documento.Contribuyente = usuario;

            _context.Documentos.Add(documento);
            await _context.SaveChangesAsync();

            return documento;
        }

        public async Task DeleteDocumentoAsync(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null)
                throw new KeyNotFoundException("Documento no encontrado");

            var filePath = Path.Combine(_environment.ContentRootPath, "Uploads", documento.RutaArchivo);
            if (File.Exists(filePath))
                File.Delete(filePath);

            _context.Documentos.Remove(documento);
            await _context.SaveChangesAsync();
        }

        public async Task<(byte[] FileContents, string ContentType, string FileName)> DownloadDocumentoAsync(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null)
                throw new KeyNotFoundException("Documento no encontrado");

            var filePath = Path.Combine(_environment.ContentRootPath, "Uploads", documento.RutaArchivo);
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Archivo no encontrado");

            var fileBytes = await File.ReadAllBytesAsync(filePath);
            var contentType = GetContentType(Path.GetExtension(documento.Nombre));

            return (fileBytes, contentType, documento.Nombre);
        }

        private string GetContentType(string extension)
        {
            return extension.ToLower() switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".txt" => "text/plain",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream",
            };
        }
    }
}