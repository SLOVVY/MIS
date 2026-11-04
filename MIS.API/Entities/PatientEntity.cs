namespace MIS.API.Entities
{
    public class PatientEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDay { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Disabilities { get; set; } = string.Empty;
        public string PassportInfo { get; set; } = string.Empty;
        public string SNILS { get; set; } = string.Empty;
        public string Polis { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime LastVisit { get; set; }
        public DateTime NextVisit { get; set; }
        public DiseaseEntity? CurrentDisease { get; set; }
        public DoctorEntity? DoctorEntity { get; set; }
        public Guid? DoctorId { get; set; }
        public Guid? DiseaseId { get; set; }

        // хорошо бы ещё историю болезней, но нужно менять/добавлять сущности
    }
}
