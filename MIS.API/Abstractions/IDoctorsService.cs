using MIS.API.Models;

namespace MIS.API.Abstractions
{
    public interface IDoctorsService
    {
        Task<(List<string> errors, Guid? id)> CreateDoctor(string firstName, string lastName, string specialization, int experience, int cabinetNumber, CancellationToken ct = default);
        Task<bool> DeleteDoctor(Guid id, CancellationToken ct = default);
        Task<List<Doctor>> GetAllDoctors(CancellationToken ct = default);
        Task<Doctor?> GetDoctorById(Guid id, CancellationToken ct = default);
        Task<List<Doctor>> GetDoctorByName(string firstName, string lastName, CancellationToken ct = default);
        Task<List<Doctor>> GetDoctorBySpecialization(string specialization, CancellationToken ct = default);
        Task<bool> UpdateDoctor(Doctor doctor, CancellationToken ct = default);
    }
}