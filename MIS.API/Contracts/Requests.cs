namespace MIS.API.Contracts
{
    public record CreatePatientRequest(
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
        Guid? DoctorId,
        Guid? DiseaseId
    );

    public record UpdatePatientRequest(
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

    public record CreateDoctorRequest(
        string FirstName,
        string LastName,
        string Specialization,
        int Experience,
        int CabinetNumber
    );

    public record UpdateDoctorRequest(
        string FirstName,
        string LastName,
        string Specialization,
        int Experience,
        int CabinetNumber
    );

    public record CreateDiseaseRequest(
        string Name,
        string Symptoms,
        string Medicaments,
        string Recommendations
    );

    public record UpdateDiseaseRequest(
        string Name,
        string Symptoms,
        string Medicaments,
        string Recommendations
    );
}
