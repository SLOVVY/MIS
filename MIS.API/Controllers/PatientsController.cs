using Microsoft.AspNetCore.Mvc;
using MIS.API.Contracts;
using MIS.API.Models;
using MIS.API.Abstractions;

namespace MIS.API.Controllers
{
    [ApiController]
    [Route("api/patients")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientsService _service;

        public PatientsController(IPatientsService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreatePatientRequest request, CancellationToken ct)
        {
            var (errors, id) = await _service.CreatePatient(
                request.FirstName, request.LastName, request.BirthDay,
                request.Address, request.Disabilities, request.PassportInfo,
                request.SNILS, request.Polis, request.PhoneNumber,
                request.LastVisit, request.NextVisit,
                request.DoctorId, request.DiseaseId, ct);

            if (errors.Count > 0)
                return BadRequest(new { errors });

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var patients = await _service.GetAllPatients(ct);
            return Ok(patients);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var patient = await _service.GetPatientDetailsAsync(id, ct);
            return patient is null ? NotFound() : Ok(patient);
        }

        [HttpGet("search/passport")]
        public async Task<IActionResult> GetByPassport(
            [FromQuery] string passportInfo, CancellationToken ct)
        {
            var patient = await _service.GetPatientByPassport(passportInfo, ct);
            return patient is null ? NotFound() : Ok(patient);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id, [FromBody] UpdatePatientRequest request, CancellationToken ct)
        {
            var existing = await _service.GetPatientById(id, ct);
            if (existing is null) return NotFound();

            var patientToUpdate = Patient.Restore(
                id, request.FirstName, request.LastName, request.BirthDay,
                request.Address, request.Disabilities, request.PassportInfo,
                request.SNILS, request.Polis, request.PhoneNumber,
                request.LastVisit, request.NextVisit,
                existing.DoctorId, existing.DiseaseId);

            var success = await _service.UpdatePatient(patientToUpdate, ct);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var success = await _service.DeletePatient(id, ct);
            return success ? NoContent() : NotFound();
        }

        [HttpPost("{id:guid}/doctor/{doctorId:guid}")]
        public async Task<IActionResult> AssignDoctor(
            Guid id, Guid doctorId, CancellationToken ct)
        {
            var success = await _service.AssignDoctorAsync(id, doctorId, ct);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}/doctor")]
        public async Task<IActionResult> RemoveDoctor(Guid id, CancellationToken ct)
        {
            var success = await _service.RemoveDoctorAsync(id, ct);
            return success ? NoContent() : NotFound();
        }

        [HttpPost("{id:guid}/disease/{diseaseId:guid}")]
        public async Task<IActionResult> AssignDisease(
            Guid id, Guid diseaseId, CancellationToken ct)
        {
            var success = await _service.AssignDiseaseAsync(id, diseaseId, ct);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}/disease")]
        public async Task<IActionResult> RemoveDisease(Guid id, CancellationToken ct)
        {
            var success = await _service.RemoveDiseaseAsync(id, ct);
            return success ? NoContent() : NotFound();
        }
    }
}
