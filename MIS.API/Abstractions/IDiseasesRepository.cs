using MIS.API.Entities;
using MIS.API.Models;

namespace MIS.API.Abstractions
{
    public interface IDiseasesRepository
    {
        Task<Guid> CreateDiseaseAsync(Disease disease, CancellationToken ct = default);
        Task<bool> DeleteDiseaseAsync(Guid id, CancellationToken ct = default);
        Task<DiseaseEntity?> GetDiseaseByIdAsync(Guid id, CancellationToken ct = default);
        IQueryable<DiseaseEntity> GetDiseasesQueryAsync();
        Task<bool> IsDiseaseExistsAsync(Guid id, CancellationToken ct = default);
        Task<bool> UpdateDiseaseAsync(Disease disease, CancellationToken ct = default);
    }
}