using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MIS.API.Entities;

namespace MIS.API.DbConfigurations
{
    public class DiseaseEntityConfiguration : IEntityTypeConfiguration<DiseaseEntity>
    {
        public void Configure(EntityTypeBuilder<DiseaseEntity> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name).IsRequired().HasMaxLength(200);
            builder.Property(d => d.Symptoms).IsRequired().HasMaxLength(2000);
            builder.Property(d => d.Medicaments).IsRequired().HasMaxLength(2000);
            builder.Property(d => d.Recommendations).IsRequired().HasMaxLength(2000);
        }
    }
}
