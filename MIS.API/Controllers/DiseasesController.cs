using Microsoft.AspNetCore.Mvc;
using MIS.API.Contracts;
using MIS.API.Models;
using MIS.API.Abstractions;

namespace MIS.API.Controllers
{
    [ApiController]
    [Route("api/diseases")]
    public class DiseasesController : ControllerBase
    {
        private readonly IDiseasesService _service;

        public DiseasesController(IDiseasesService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateDiseaseRequest request, CancellationToken ct)
        {
            var (errors, id) = await _service.CreateDisease(
                request.Name, request.Symptoms, request.Medicaments, request.Recommendations, ct);

            if (errors.Count > 0)
                return BadRequest(new { errors });

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var diseases = await _service.GetAllDiseases(ct);
            return Ok(diseases);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var disease = await _service.GetDiseaseById(id, ct);
            return disease is null ? NotFound() : Ok(disease);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id, [FromBody] UpdateDiseaseRequest request, CancellationToken ct)
        {
            var existing = await _service.GetDiseaseById(id, ct);
            if (existing is null) return NotFound();

            var diseaseToUpdate = Disease.Restore(
                id, request.Name, request.Symptoms, request.Medicaments, request.Recommendations);

            var success = await _service.UpdateDisease(diseaseToUpdate, ct);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var success = await _service.DeleteDisease(id, ct);
            return success ? NoContent() : NotFound();
        }
    }
}
