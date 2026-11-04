using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MIS.API.Entities;

namespace MIS.API.DbConfigurations
{
    public class PatientEntityConfiguration : IEntityTypeConfiguration<PatientEntity>
    {
        public void Configure(EntityTypeBuilder<PatientEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Address).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Disabilities).HasMaxLength(1000);
            builder.Property(p => p.PassportInfo).HasMaxLength(10);
            builder.Property(p => p.SNILS).HasMaxLength(11);
            builder.Property(p => p.Polis).HasMaxLength(16);
            builder.Property(p => p.PhoneNumber).HasMaxLength(11);

            builder.HasIndex(p => p.PassportInfo).IsUnique();
            builder.HasIndex(p => p.SNILS).IsUnique();
            builder.HasIndex(p => p.Polis).IsUnique();
            builder.HasIndex(p => p.PhoneNumber).IsUnique();

            builder.HasOne(p => p.DoctorEntity)
               .WithMany(d => d.Patients)
               .HasForeignKey(p => p.DoctorId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.CurrentDisease)
                   .WithMany(d => d.Patients)
                   .HasForeignKey(p => p.DiseaseId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
