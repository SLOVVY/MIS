using Microsoft.EntityFrameworkCore;
using MIS.API.Abstractions;
using MIS.API.Entities;
using MIS.API.Models;

namespace MIS.API.Repositories
{
    public class DiseasesRepository : IDiseasesRepository
    {
        private readonly AppDbContext _dbContext;

        public DiseasesRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> CreateDiseaseAsync(Disease disease, CancellationToken ct = default)
        {
            var diseaseEntity = new DiseaseEntity
            {
                Id = disease.Id,
                Name = disease.Name,
                Symptoms = disease.Symptoms,
                Medicaments = disease.Medicaments,
                Recommendations = disease.Recommendations,
            };

            await _dbContext.AddAsync(diseaseEntity);
            await _dbContext.SaveChangesAsync(ct);

            return disease.Id;
        }

        public IQueryable<DiseaseEntity> GetDiseasesQueryAsync() => _dbContext.Diseases.AsNoTracking();

        public Task<bool> IsDiseaseExistsAsync(Guid id, CancellationToken ct = default) =>
            _dbContext.Diseases.Where(x => x.Id == id).AnyAsync(ct);

        public Task<DiseaseEntity?> GetDiseaseByIdAsync(Guid id, CancellationToken ct = default) =>
            _dbContext.Diseases
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);


        public async Task<bool> UpdateDiseaseAsync(Disease disease, CancellationToken ct = default)
        {
            var affected = await _dbContext.Diseases
                .Where(x => x.Id == disease.Id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.Name, disease.Name)
                .SetProperty(x => x.Symptoms, disease.Symptoms)
                .SetProperty(x => x.Recommendations, disease.Recommendations)
                .SetProperty(x => x.Medicaments, disease.Medicaments), ct);

            return affected > 0;
        }

        public async Task<bool> DeleteDiseaseAsync(Guid id, CancellationToken ct = default)
        {
            var affected = await _dbContext.Diseases
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(ct);

            return affected > 0;
        }
    }
}
