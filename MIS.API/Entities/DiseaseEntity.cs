namespace MIS.API.Entities
{
    public class DiseaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Symptoms { get; set; } = string.Empty;
        public string Medicaments { get; set; } = string.Empty;
        public string Recommendations { get; set; } = string.Empty;
        public List<PatientEntity> Patients { get; set; } = new List<PatientEntity>();
    }
}
