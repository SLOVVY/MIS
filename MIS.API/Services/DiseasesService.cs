using MIS.API.Entities;
using MIS.API.Models;
using Microsoft.EntityFrameworkCore;
using MIS.API.Abstractions;

namespace MIS.API.Services
{
    public class DiseasesService : IDiseasesService
    {
        private readonly IDiseasesRepository _repository;
        private readonly ILogger<DiseasesService> _logger;

        public DiseasesService(IDiseasesRepository repository, ILogger<DiseasesService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<(List<string> errors, Guid? id)> CreateDisease(
            string name, string symptoms, string medicaments, string recommendations,
            CancellationToken ct = default)
        {
            _logger.LogInformation("Попытка создания болезни: {Name}", name);

            var (errors, disease) = Disease.CreateDisease(name, symptoms, medicaments, recommendations);

            if (errors.Count > 0)
            {
                _logger.LogWarning("Ошибка валидации при создании болезни: {Errors}", string.Join("; ", errors));
                return (errors, null);
            }

            var id = await _repository.CreateDiseaseAsync(disease!, ct);
            _logger.LogInformation("Болезнь успешно создана. Id: {DiseaseId}", id);
            return ([], id);
        }

        public async Task<Disease?> GetDiseaseById(Guid id, CancellationToken ct = default)
        {
            var entity = await _repository.GetDiseaseByIdAsync(id, ct);

            if (entity is null)
            {
                _logger.LogDebug("Болезнь с Id {DiseaseId} не найдена", id);
                return null;
            }

            return MapToDomain(entity);
        }

        public async Task<List<Disease>> GetAllDiseases(CancellationToken ct = default)
        {
            _logger.LogDebug("Запрос списка всех болезней");

            var entities = await _repository.GetDiseasesQueryAsync()
                .OrderBy(x => x.Name)
                .ToListAsync(ct);

            return entities.Select(MapToDomain).ToList();
        }

        public async Task<bool> UpdateDisease(Disease disease, CancellationToken ct = default)
        {
            _logger.LogInformation("Попытка обновления данных болезни {DiseaseId}", disease.Id);
            var result = await _repository.UpdateDiseaseAsync(disease, ct);

            if (result) _logger.LogInformation("Данные болезни {DiseaseId} успешно обновлены", disease.Id);
            else _logger.LogWarning("Болезнь {DiseaseId} не найдена для обновления", disease.Id);

            return result;
        }

        public async Task<bool> DeleteDisease(Guid id, CancellationToken ct = default)
        {
            _logger.LogInformation("Попытка удаления болезни {DiseaseId}", id);
            var result = await _repository.DeleteDiseaseAsync(id, ct);

            if (result) _logger.LogInformation("Болезнь {DiseaseId} успешно удалена", id);
            else _logger.LogWarning("Болезнь {DiseaseId} не найдена для удаления", id);

            return result;
        }

        private static Disease MapToDomain(DiseaseEntity entity) =>
            Disease.Restore(entity.Id, entity.Name, entity.Symptoms,
                entity.Medicaments, entity.Recommendations);
    }
}