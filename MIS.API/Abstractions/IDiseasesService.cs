using MIS.API.Models;

namespace MIS.API.Abstractions
{
    public interface IDiseasesService
    {
        Task<(List<string> errors, Guid? id)> CreateDisease(string name, string symptoms, string medicaments, string recommendations, CancellationToken ct = default);
        Task<bool> DeleteDisease(Guid id, CancellationToken ct = default);
        Task<List<Disease>> GetAllDiseases(CancellationToken ct = default);
        Task<Disease?> GetDiseaseById(Guid id, CancellationToken ct = default);
        Task<bool> UpdateDisease(Disease disease, CancellationToken ct = default);
    }
}