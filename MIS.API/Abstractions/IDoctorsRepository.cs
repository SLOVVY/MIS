using MIS.API.Entities;
using MIS.API.Models;

namespace MIS.API.Abstractions
{
    public interface IDoctorsRepository
    {
        Task<Guid> CreateDoctorAsync(Doctor doctor, CancellationToken ct = default);
        Task<bool> DeleteDoctorAsync(Guid id, CancellationToken ct = default);
        Task<DoctorEntity?> GetDoctorByIdAsync(Guid id, CancellationToken ct = default);
        IQueryable<DoctorEntity> GetDoctorsQueryAsync();
        Task<bool> IsDoctorExistsAsync(Guid id, CancellationToken ct = default);
        Task<bool> UpdateDoctorAsync(Doctor doctor, CancellationToken ct = default);
    }
}