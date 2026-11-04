using Microsoft.AspNetCore.Mvc;
using MIS.API.Contracts;
using MIS.API.Models;
using MIS.API.Abstractions;

namespace MIS.API.Controllers
{
    [ApiController]
    [Route("api/doctors")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorsService _service;

        public DoctorsController(IDoctorsService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateDoctorRequest request, CancellationToken ct)
        {
            var (errors, id) = await _service.CreateDoctor(
                request.FirstName, request.LastName, request.Specialization,
                request.Experience, request.CabinetNumber, ct);

            if (errors.Count > 0)
                return BadRequest(new { errors });

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var doctors = await _service.GetAllDoctors(ct);
            return Ok(doctors);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var doctor = await _service.GetDoctorById(id, ct);
            return doctor is null ? NotFound() : Ok(doctor);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? firstName,
            [FromQuery] string? lastName,
            [FromQuery] string? specialization,
            CancellationToken ct)
        {
            if (!string.IsNullOrWhiteSpace(specialization))
                return Ok(await _service.GetDoctorBySpecialization(specialization, ct));

            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
                return Ok(await _service.GetDoctorByName(firstName, lastName, ct));

            return BadRequest(new { error = "Укажите специализацию или имя и фамилию для поиска." });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id, [FromBody] UpdateDoctorRequest request, CancellationToken ct)
        {
            var existing = await _service.GetDoctorById(id, ct);
            if (existing is null) return NotFound();

            var doctorToUpdate = Doctor.Restore(
                id, request.FirstName, request.LastName, request.Specialization,
                request.Experience, request.CabinetNumber);

            var success = await _service.UpdateDoctor(doctorToUpdate, ct);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var success = await _service.DeleteDoctor(id, ct);
            return success ? NoContent() : NotFound();
        }
    }
}