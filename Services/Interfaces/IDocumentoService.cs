using BackEnd.Models;

namespace BackEnd.Services.Interfaces
{
    public interface IDocumentoService
    {
        Task<IEnumerable<Documento>> GetDocumentosAsync();
        Task<Documento?> GetDocumentoByIdAsync(int id);
        Task<IEnumerable<Documento>> GetDocumentosByContribuyenteAsync(int contribuyenteId);
        Task<Documento> CreateDocumentoAsync(Documento documento, IFormFile file);
        Task DeleteDocumentoAsync(int id);
        Task<(byte[] FileContents, string ContentType, string FileName)> DownloadDocumentoAsync(int id);
    }
}