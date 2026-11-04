using MIS.API.Contracts;
using MIS.API.Models;

namespace MIS.API.Abstractions
{
    public interface IPatientsService
    {
        Task<bool> AssignDiseaseAsync(Guid patientId, Guid diseaseId, CancellationToken ct = default);
        Task<bool> AssignDoctorAsync(Guid patientId, Guid doctorId, CancellationToken ct = default);
        Task<(List<string> errors, Guid? id)> CreatePatient(string firstName, string lastName, DateTime birthDay, string address, string disabilities, string passportInfo, string snils, string polis, string phoneNumber, DateTime lastVisit, DateTime nextVisit, Guid? doctorId, Guid? diseaseId, CancellationToken ct = default);
        Task<bool> DeletePatient(Guid id, CancellationToken ct = default);
        Task<List<Patient>> GetAllPatients(CancellationToken ct = default);
        Task<Patient?> GetPatientById(Guid id, CancellationToken ct = default);
        Task<List<Patient>> GetPatientByName(string firstName, string lastName, CancellationToken ct = default);
        Task<Patient?> GetPatientByPassport(string passportInfo, CancellationToken ct = default);
        Task<PatientDetailsDto?> GetPatientDetailsAsync(Guid id, CancellationToken ct = default);
        Task<bool> RemoveDiseaseAsync(Guid patientId, CancellationToken ct = default);
        Task<bool> RemoveDoctorAsync(Guid patientId, CancellationToken ct = default);
        Task<bool> UpdatePatient(Patient patient, CancellationToken ct = default);
    }
}