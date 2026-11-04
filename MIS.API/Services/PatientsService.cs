using Microsoft.EntityFrameworkCore;
using MIS.API.Abstractions;
using MIS.API.Contracts;
using MIS.API.Entities;
using MIS.API.Models;

namespace MIS.API.Services
{
    public class PatientsService : IPatientsService
    {
        private readonly IPatientsRepository _patientsRepository;
        private readonly IDoctorsRepository _doctorsRepository;
        private readonly IDiseasesRepository _diseasesRepository;
        private readonly ILogger<PatientsService> _logger;

        public PatientsService(
            IPatientsRepository patientsRepository,
            IDoctorsRepository doctorsRepository,
            IDiseasesRepository diseasesRepository,
            ILogger<PatientsService> logger)
        {
            _patientsRepository = patientsRepository;
            _doctorsRepository = doctorsRepository;
            _diseasesRepository = diseasesRepository;
            _logger = logger;
        }

        public async Task<(List<string> errors, Guid? id)> CreatePatient(
            string firstName, string lastName, DateTime birthDay,
            string address, string disabilities, string passportInfo,
            string snils, string polis, string phoneNumber,
            DateTime lastVisit, DateTime nextVisit, Guid? doctorId, Guid? diseaseId,
            CancellationToken ct = default)
        {
            _logger.LogInformation("Попытка создания пациента: {FirstName} {LastName}", firstName, lastName);

            var (errors, patient) = Patient.CreatePatient(
                firstName, lastName, birthDay, address, disabilities,
                passportInfo, snils, polis, phoneNumber, lastVisit, nextVisit, doctorId, diseaseId);

            if (errors.Count > 0)
            {
                _logger.LogWarning("Ошибка валидации при создании пациента: {Errors}", string.Join("; ", errors));
                return (errors, null);
            }

            if (doctorId.HasValue && !await _doctorsRepository.IsDoctorExistsAsync(doctorId.Value, ct))
            {
                _logger.LogWarning("Указанный врач {DoctorId} не найден при создании пациента", doctorId.Value);
                return (["Указанный врач не найден."], null);
            }

            if (diseaseId.HasValue && !await _diseasesRepository.IsDiseaseExistsAsync(diseaseId.Value, ct))
            {
                _logger.LogWarning("Указанная болезнь {DiseaseId} не найдена при создании пациента", diseaseId.Value);
                return (["Указанная болезнь не найдена."], null);
            }

            var id = await _patientsRepository.CreatePatientAsync(patient!, ct);
            _logger.LogInformation("Пациент успешно создан. Id: {PatientId}", id);
            return ([], id);
        }

        public async Task<Patient?> GetPatientById(Guid id, CancellationToken ct = default)
        {
            var entity = await _patientsRepository.GetPatientByIdAsync(id, ct);
            if (entity is null)
            {
                _logger.LogDebug("Пациент с Id {PatientId} не найден", id);
                return null;
            }
            return MapToDomain(entity);
        }

        public async Task<List<Patient>> GetAllPatients(CancellationToken ct = default)
        {
            _logger.LogDebug("Запрос списка всех пациентов");

            var entities = await _patientsRepository.GetPatientsQueryAsync()
                .OrderBy(x => x.LastVisit)
                .ToListAsync(ct);

            return entities.Select(MapToDomain).ToList();
        }

        public async Task<List<Patient>> GetPatientByName(
            string firstName, string lastName, CancellationToken ct = default)
        {
            _logger.LogDebug("Поиск пациента по имени: {FirstName} {LastName}", firstName, lastName);

            var entities = await _patientsRepository.GetPatientsQueryAsync()
                .Where(x => x.FirstName == firstName && x.LastName == lastName)
                .OrderBy(x => x.LastName)
                .ToListAsync(ct);

            return entities.Select(MapToDomain).ToList();
        }

        public async Task<Patient?> GetPatientByPassport(
            string passportInfo, CancellationToken ct = default)
        {
            _logger.LogDebug("Поиск пациента по паспорту");

            var entity = await _patientsRepository.GetPatientsQueryAsync()
                .FirstOrDefaultAsync(x => x.PassportInfo == passportInfo, ct);
            return entity is null ? null : MapToDomain(entity);
        }

        public async Task<bool> UpdatePatient(Patient patient, CancellationToken ct = default)
        {
            _logger.LogInformation("Попытка обновления данных пациента {PatientId}", patient.Id);
            var result = await _patientsRepository.UpdatePatientInfoAsync(patient, ct);

            if (result) _logger.LogInformation("Данные пациента {PatientId} успешно обновлены", patient.Id);
            else _logger.LogWarning("Пациент {PatientId} не найден для обновления", patient.Id);

            return result;
        }

        public async Task<bool> DeletePatient(Guid id, CancellationToken ct = default)
        {
            _logger.LogInformation("Попытка удаления пациента {PatientId}", id);
            var result = await _patientsRepository.DeletePatientAsync(id, ct);

            if (result) _logger.LogInformation("Пациент {PatientId} успешно удален", id);
            else _logger.LogWarning("Пациент {PatientId} не найден для удаления", id);

            return result;
        }

        public async Task<bool> AssignDoctorAsync(Guid patientId, Guid doctorId, CancellationToken ct = default)
        {
            _logger.LogInformation("Назначение врача {DoctorId} пациенту {PatientId}", doctorId, patientId);

            var entity = await _patientsRepository.GetPatientByIdAsync(patientId, ct);
            if (entity is null)
            {
                _logger.LogWarning("Пациент {PatientId} не найден при попытке назначить врача", patientId);
                return false;
            }

            if (!await _doctorsRepository.IsDoctorExistsAsync(doctorId, ct))
            {
                _logger.LogWarning("Врач {DoctorId} не найден в БД", doctorId);
                throw new InvalidOperationException("Врач не найден.");
            }

            var result = await _patientsRepository.UpdateAssignedDoctorAsync(patientId, doctorId, ct);
            if (result) _logger.LogInformation("Врач {DoctorId} успешно назначен пациенту {PatientId}", doctorId, patientId);

            return result;
        }

        public async Task<bool> RemoveDoctorAsync(Guid patientId, CancellationToken ct = default)
        {
            _logger.LogInformation("Снятие врача с пациента {PatientId}", patientId);

            var entity = await _patientsRepository.GetPatientByIdAsync(patientId, ct);
            if (entity is null)
            {
                _logger.LogWarning("Пациент {PatientId} не найден при попытке снять врача", patientId);
                return false;
            }

            var result = await _patientsRepository.RemoveAssignedDoctorAsync(patientId, ct);
            if (result) _logger.LogInformation("Врач успешно снят с пациента {PatientId}", patientId);

            return result;
        }

        public async Task<bool> AssignDiseaseAsync(Guid patientId, Guid diseaseId, CancellationToken ct = default)
        {
            _logger.LogInformation("Назначение болезни {DiseaseId} пациенту {PatientId}", diseaseId, patientId);

            var entity = await _patientsRepository.GetPatientByIdAsync(patientId, ct);
            if (entity is null)
            {
                _logger.LogWarning("Пациент {PatientId} не найден при попытке назначить болезнь", patientId);
                return false;
            }

            if (!await _diseasesRepository.IsDiseaseExistsAsync(diseaseId, ct))
            {
                _logger.LogWarning("Болезнь {DiseaseId} не найдена в БД", diseaseId);
                throw new InvalidOperationException("Болезнь не найдена.");
            }

            var result = await _patientsRepository.UpdateAssignedDiseaseAsync(patientId, diseaseId, ct);
            if (result) _logger.LogInformation("Болезнь {DiseaseId} успешно назначена пациенту {PatientId}", diseaseId, patientId);

            return result;
        }

        public async Task<bool> RemoveDiseaseAsync(Guid patientId, CancellationToken ct = default)
        {
            _logger.LogInformation("Снятие болезни с пациента {PatientId}", patientId);

            var entity = await _patientsRepository.GetPatientByIdAsync(patientId, ct);
            if (entity is null)
            {
                _logger.LogWarning("Пациент {PatientId} не найден при попытке снять болезнь", patientId);
                return false;
            }

            var result = await _patientsRepository.RemoveAssignedDiseaseAsync(patientId, ct);
            if (result) _logger.LogInformation("Болезнь успешно снята с пациента {PatientId}", patientId);

            return result;
        }

        public async Task<PatientDetailsDto?> GetPatientDetailsAsync(Guid id, CancellationToken ct = default)
        {
            _logger.LogDebug("Запрос детальной информации о пациенте {PatientId}", id);

            return await _patientsRepository.GetPatientsQueryAsync()
                .Where(p => p.Id == id)
                .Select(p => new PatientDetailsDto(
                    p.Id, p.FirstName, p.LastName, p.BirthDay, p.Address, p.Disabilities,
                    p.PassportInfo, p.SNILS, p.Polis, p.PhoneNumber, p.LastVisit, p.NextVisit,

                    p.DoctorEntity != null ? $"{p.DoctorEntity.LastName} {p.DoctorEntity.FirstName}" : null,
                    p.DoctorEntity != null ? p.DoctorEntity.Specialization : null,
                    p.DoctorEntity != null ? p.DoctorEntity.Experience : null,
                    p.DoctorEntity != null ? p.DoctorEntity.CabinetNumber : null,

                    p.CurrentDisease != null ? p.CurrentDisease.Name : null,
                    p.CurrentDisease != null ? p.CurrentDisease.Symptoms : null,
                    p.CurrentDisease != null ? p.CurrentDisease.Recommendations : null,
                    p.CurrentDisease != null ? p.CurrentDisease.Medicaments : null
                ))
                .FirstOrDefaultAsync(ct);
        }

        private static Patient MapToDomain(PatientEntity entity) =>
            Patient.Restore(
                entity.Id, entity.FirstName, entity.LastName, entity.BirthDay,
                entity.Address, entity.Disabilities, entity.PassportInfo,
                entity.SNILS, entity.Polis, entity.PhoneNumber,
                entity.LastVisit, entity.NextVisit, entity.DoctorId, entity.DiseaseId);
    }
}
