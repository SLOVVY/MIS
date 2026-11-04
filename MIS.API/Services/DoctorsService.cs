using MIS.API.Entities;
using MIS.API.Models;
using Microsoft.EntityFrameworkCore;
using MIS.API.Abstractions;

namespace MIS.API.Services
{
    public class DoctorsService : IDoctorsService
    {
        private readonly IDoctorsRepository _repository;
        private readonly ILogger<DoctorsService> _logger;

        public DoctorsService(IDoctorsRepository repository, ILogger<DoctorsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<(List<string> errors, Guid? id)> CreateDoctor(
            string firstName, string lastName, string specialization,
            int experience, int cabinetNumber,
            CancellationToken ct = default)
        {
            _logger.LogInformation("Попытка создания врача: {FirstName} {LastName}", firstName, lastName);

            var (errors, doctor) = Doctor.CreateDoctor(
                firstName, lastName, specialization, experience, cabinetNumber);

            if (errors.Count > 0)
            {
                _logger.LogWarning("Ошибка валидации при создании врача: {Errors}", string.Join("; ", errors));
                return (errors, null);
            }

            var id = await _repository.CreateDoctorAsync(doctor!, ct);
            _logger.LogInformation("Врач успешно создан. Id: {DoctorId}", id);
            return ([], id);
        }

        public async Task<Doctor?> GetDoctorById(Guid id, CancellationToken ct = default)
        {
            var entity = await _repository.GetDoctorByIdAsync(id, ct);
            if (entity is null)
            {
                _logger.LogDebug("Врач с Id {DoctorId} не найден", id);
                return null;
            }
            return MapToDomain(entity);
        }

        public async Task<List<Doctor>> GetAllDoctors(CancellationToken ct = default)
        {
            _logger.LogDebug("Запрос списка всех врачей");

            var entities = await _repository.GetDoctorsQueryAsync()
                .OrderBy(x => x.LastName)
                .ToListAsync(ct);

            return entities.Select(MapToDomain).ToList();
        }

        public async Task<List<Doctor>> GetDoctorBySpecialization(
            string specialization, CancellationToken ct = default)
        {
            _logger.LogDebug("Поиск врачей по специализации: {Specialization}", specialization);

            var entities = await _repository.GetDoctorsQueryAsync()
                .Where(x => x.Specialization == specialization)
                .OrderBy(x => x.LastName)
                .ToListAsync(ct);

            return entities.Select(MapToDomain).ToList();
        }

        public async Task<List<Doctor>> GetDoctorByName(
            string firstName, string lastName, CancellationToken ct = default)
        {
            _logger.LogDebug("Поиск врача по имени: {FirstName} {LastName}", firstName, lastName);

            var entities = await _repository.GetDoctorsQueryAsync()
                .Where(x => x.FirstName == firstName && x.LastName == lastName)
                .OrderBy(x => x.LastName)
                .ToListAsync(ct);

            return entities.Select(MapToDomain).ToList();
        }

        public async Task<bool> UpdateDoctor(Doctor doctor, CancellationToken ct = default)
        {

            _logger.LogInformation("Попытка обновления данных врача {DoctorId}", doctor.Id);
            var result = await _repository.UpdateDoctorAsync(doctor, ct);

            if (result) _logger.LogInformation("Данные врача {DoctorId} успешно обновлены", doctor.Id);
            else _logger.LogWarning("Врач {DoctorId} не найден для обновления", doctor.Id);

            return result;
        }

        public async Task<bool> DeleteDoctor(Guid id, CancellationToken ct = default)
        {
            _logger.LogInformation("Попытка удаления врача {DoctorId}", id);
            var result = await _repository.DeleteDoctorAsync(id, ct);

            if (result) _logger.LogInformation("Врач {DoctorId} успешно удален", id);
            else _logger.LogWarning("Врач {DoctorId} не найден для удаления", id);

            return result;
        }

        private static Doctor MapToDomain(DoctorEntity entity) =>
            Doctor.Restore(entity.Id, entity.FirstName, entity.LastName,
                entity.Specialization, entity.Experience, entity.CabinetNumber);
    }
}