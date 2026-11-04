using MIS.API.Entities;
using MIS.API.Models;

namespace MIS.API.Abstractions
{
    public interface IPatientsRepository
    {
        Task<Guid> CreatePatientAsync(Patient patient, CancellationToken ct = default);
        Task<bool> DeletePatientAsync(Guid id, CancellationToken ct = default);
        Task<PatientEntity?> GetPatientByIdAsync(Guid id, CancellationToken ct = default);
        IQueryable<PatientEntity> GetPatientsQueryAsync();
        Task<bool> RemoveAssignedDiseaseAsync(Guid patientId, CancellationToken ct = default);
        Task<bool> RemoveAssignedDoctorAsync(Guid patientId, CancellationToken ct = default);
        Task<bool> UpdateAssignedDiseaseAsync(Guid patientId, Guid diseaseId, CancellationToken ct = default);
        Task<bool> UpdateAssignedDoctorAsync(Guid patientId, Guid doctorId, CancellationToken ct = default);
        Task<bool> UpdatePatientInfoAsync(Patient patient, CancellationToken ct = default);
    }
}