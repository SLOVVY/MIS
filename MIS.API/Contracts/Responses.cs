namespace MIS.API.Contracts
{
    public record PatientDetailsDto(
       Guid Id,
       string FirstName,
       string LastName,
       DateTime BirthDay,
       string Address,
       string Disabilities,
       string PassportInfo,
       string SNILS,
       string Polis,
       string PhoneNumber,
       DateTime LastVisit,
       DateTime NextVisit,
       
       string? DoctorFullName,
       string? DoctorSpecialization,
       int? DoctorExperience,
       int? DoctorCabinetNumber,
       
       string? DiseaseName,
       string? DiseaseSymptoms,
       string? DiseaseRecommendations,
       string? DiseaseMedicine
   );

    public record PatientDto(
        Guid Id,
        string FirstName,
        string LastName,
        DateTime BirthDay,
        string Address,
        string Disabilities,
        string PassportInfo,
        string SNILS,
        string Polis,
        string PhoneNumber,
        DateTime LastVisit,
        DateTime NextVisit
    );

    public record DoctorDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Specialization,
    int Experience,
    int CabinetNumber
);

    public record DiseaseDto(
        Guid Id,
        string Name,
        string Symptoms,
        string Medicaments,
        string Recommendations
    );
}
