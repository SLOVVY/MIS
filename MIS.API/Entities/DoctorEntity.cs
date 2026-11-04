namespace MIS.API.Entities
{
    public class DoctorEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public int Experience { get; set; }
        public int CabinetNumber { get; set; }

        public List<PatientEntity> Patients { get; set; } = new List<PatientEntity>();
    }
}
