using Microsoft.EntityFrameworkCore;
using MIS.API.Abstractions;
using MIS.API.Entities;
using MIS.API.Models;

namespace MIS.API.Repositories
{
    public class DoctorsRepository : IDoctorsRepository
    {
        private readonly AppDbContext _dbContext;

        public DoctorsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> CreateDoctorAsync(Doctor doctor, CancellationToken ct = default)
        {
            var doctorEntity = new DoctorEntity
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Experience = doctor.Experience,
                Specialization = doctor.Specialization,
                CabinetNumber = doctor.CabinetNumber
            };

            await _dbContext.AddAsync(doctorEntity);
            await _dbContext.SaveChangesAsync(ct);

            return doctor.Id;
        }

        public IQueryable<DoctorEntity> GetDoctorsQueryAsync() => _dbContext.Doctors.AsNoTracking();

        public Task<bool> IsDoctorExistsAsync(Guid id, CancellationToken ct = default) =>
            _dbContext.Doctors.Where(x => x.Id == id).AnyAsync(ct);

        public Task<DoctorEntity?> GetDoctorByIdAsync(Guid id, CancellationToken ct = default) =>
            _dbContext.Doctors
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<bool> UpdateDoctorAsync(Doctor doctor, CancellationToken ct = default)
        {
            var affected = await _dbContext.Doctors
                .Where(x => x.Id == doctor.Id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.FirstName, doctor.FirstName)
                .SetProperty(x => x.LastName, doctor.LastName)
                .SetProperty(x => x.Specialization, doctor.Specialization)
                .SetProperty(x => x.Experience, doctor.Experience)
                .SetProperty(x => x.CabinetNumber, doctor.CabinetNumber), ct);

            return affected > 0;
        }

        public async Task<bool> DeleteDoctorAsync(Guid id, CancellationToken ct = default)
        {
            var affected = await _dbContext.Doctors
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(ct);

            return affected > 0;
        }
    }
}
