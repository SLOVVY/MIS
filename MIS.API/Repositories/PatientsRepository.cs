using Microsoft.EntityFrameworkCore;
using MIS.API.Abstractions;
using MIS.API.Entities;
using MIS.API.Models;

namespace MIS.API.Repositories
{
    public class PatientsRepository : IPatientsRepository
    {
        private readonly AppDbContext _dbContext;

        public PatientsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> CreatePatientAsync(Patient patient, CancellationToken ct = default)
        {
            var patientEntity = new PatientEntity
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                BirthDay = patient.BirthDay,
                Address = patient.Address,
                Disabilities = patient.Disabilities,
                PassportInfo = patient.PassportInfo,
                SNILS = patient.SNILS,
                Polis = patient.Polis,
                PhoneNumber = patient.PhoneNumber,
                LastVisit = patient.LastVisit,
                NextVisit = patient.NextVisit,
                DoctorId = patient.DoctorId,
                DiseaseId = patient.DiseaseId
            };

            await _dbContext.AddAsync(patientEntity);
            await _dbContext.SaveChangesAsync(ct);

            return patient.Id;
        }

        public IQueryable<PatientEntity> GetPatientsQueryAsync() => _dbContext.Patients.AsNoTracking();

        public Task<PatientEntity?> GetPatientByIdAsync(Guid id, CancellationToken ct = default) =>
            _dbContext.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<bool> UpdatePatientInfoAsync(Patient patient, CancellationToken ct = default)
        {
            var affected = await _dbContext.Patients
                .Where(x => x.Id == patient.Id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.FirstName, patient.FirstName)
                .SetProperty(x => x.LastName, patient.LastName)
                .SetProperty(x => x.BirthDay, patient.BirthDay)
                .SetProperty(x => x.Address, patient.Address)
                .SetProperty(x => x.Disabilities, patient.Disabilities)
                .SetProperty(x => x.PassportInfo, patient.PassportInfo)
                .SetProperty(x => x.SNILS, patient.SNILS)
                .SetProperty(x => x.Polis, patient.Polis)
                .SetProperty(x => x.PhoneNumber, patient.PhoneNumber)
                .SetProperty(x => x.LastVisit, patient.LastVisit)
                .SetProperty(x => x.NextVisit, patient.NextVisit), ct);

            return affected > 0;
        }

        public async Task<bool> UpdateAssignedDoctorAsync(Guid patientId, Guid doctorId, CancellationToken ct = default)
        {
            var affected = await _dbContext.Patients
                .Where(x => x.Id == patientId)
                .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.DoctorId, doctorId), ct);

            return affected > 0;
        }

        public async Task<bool> UpdateAssignedDiseaseAsync(Guid patientId, Guid diseaseId, CancellationToken ct = default)
        {
            var affected = await _dbContext.Patients
                .Where(x => x.Id == patientId)
                .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.DiseaseId, diseaseId), ct);

            return affected > 0;
        }

        public async Task<bool> RemoveAssignedDoctorAsync(Guid patientId, CancellationToken ct = default)
        {
            var affected = await _dbContext.Patients
                .Where(x => x.Id == patientId)
                .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.DoctorId, (Guid?)null), ct);

            return affected > 0;
        }

        public async Task<bool> RemoveAssignedDiseaseAsync(Guid patientId, CancellationToken ct = default)
        {
            var affected = await _dbContext.Patients
                .Where(x => x.Id == patientId)
                .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.DiseaseId, (Guid?)null), ct);

            return affected > 0;
        }

        public async Task<bool> DeletePatientAsync(Guid id, CancellationToken ct = default)
        {
            var affected = await _dbContext.Patients
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(ct);

            return affected > 0;
        }
    }
}
