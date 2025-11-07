using Microsoft.AspNetCore.Mvc;
using BackEnd.Models;
using BackEnd.Services.Interfaces;
using BackEnd.DTOs;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitasController : ControllerBase
    {
        private readonly ICitaService _citaService;

        public CitasController(ICitaService citaService)
        {
            _citaService = citaService;
        }

        /// <summary>
        /// Obtiene todas las citas registradas en el sistema
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cita>>> GetCitas()
        {
            var citas = await _citaService.GetCitasAsync();
            return Ok(citas);
        }

        /// <summary>
        /// Obtiene una cita específica por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Cita>> GetCita(int id)
        {
            var cita = await _citaService.GetCitaByIdAsync(id);
            if (cita == null)
            {
                return NotFound();
            }
            return Ok(cita);
        }

        /// <summary>
        /// Obtiene todas las citas asignadas a un contador específico
        /// </summary>
        [HttpGet("Contador/{contadorId}")]
        public async Task<ActionResult<IEnumerable<Cita>>> GetCitasByContador(int contadorId)
        {
            var citas = await _citaService.GetCitasByContadorAsync(contadorId);
            return Ok(citas);
        }

        /// <summary>
        /// Obtiene todas las citas de un contribuyente específico
        /// </summary>
        [HttpGet("Contribuyente/{contribuyenteId}")]
        public async Task<ActionResult<IEnumerable<Cita>>> GetCitasByContribuyente(int contribuyenteId)
        {
            var citas = await _citaService.GetCitasByContribuyenteAsync(contribuyenteId);
            return Ok(citas);
        }

        /// <summary>
        /// Crea una nueva cita en el sistema
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Cita>> CreateCita(CitaCreateDto citaDto)
        {
            try
            {
                var cita = new Cita
                {
                    ContadorId = citaDto.ContadorId,
                    ContribuyenteId = citaDto.ContribuyenteId,
                    Fecha = citaDto.Fecha,
                    Hora = citaDto.Hora,
                    Estado = citaDto.Estado,
                    Notas = citaDto.Notas,
                    Contador = null!,
                    Contribuyente = null!
                };

                var nuevaCita = await _citaService.CreateCitaAsync(cita);
                return CreatedAtAction(nameof(GetCita), new { id = nuevaCita.Id }, nuevaCita);
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
        /// Actualiza una cita existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCita(int id, Cita cita)
        {
            try
            {
                await _citaService.UpdateCitaAsync(id, cita);
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
        /// Elimina una cita del sistema
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCita(int id)
        {
            try
            {
                await _citaService.DeleteCitaAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}