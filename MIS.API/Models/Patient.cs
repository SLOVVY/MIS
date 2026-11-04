using MIS.API.Entities;
using System.Text.RegularExpressions;

namespace MIS.API.Models
{
    public class Patient
    {
        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime BirthDay { get; }
        public string Address { get; }
        public string Disabilities { get; }
        public string PassportInfo { get; }
        public string SNILS { get; }
        public string Polis { get; }
        public string PhoneNumber { get; }
        public DateTime LastVisit { get; }
        public DateTime NextVisit { get; }


        public Guid? DoctorId { get; private set; }
        public Guid? DiseaseId { get; private set; }

        private Patient(
            Guid id,
            string firstName,
            string lastName,
            DateTime birthDay,
            string address,
            string disabilities,
            string passportInfo,
            string snils,
            string polis,
            string phoneNumber,
            DateTime lastVisit,
            DateTime nextVisit,
            Guid? doctorId,
            Guid? diseaseId)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            BirthDay = birthDay;
            Address = address;
            Disabilities = disabilities;
            PassportInfo = passportInfo;
            SNILS = snils;
            Polis = polis;
            PhoneNumber = phoneNumber;
            LastVisit = lastVisit;
            NextVisit = nextVisit;
            DoctorId = doctorId;
            DiseaseId = diseaseId;
        }

        public static (List<string> errors, Patient? patient) CreatePatient(
            string firstName,
            string lastName,
            DateTime birthDay,
            string address,
            string disabilities,
            string passportInfo,
            string snils,
            string polis,
            string phoneNumber,
            DateTime lastVisit,
            DateTime nextVisit,
            Guid? doctorId,
            Guid? diseaseId
            )
        {
            List<string> errors = [];

            void ValidateString(string value, string fieldName, int maxLength)
            {
                if (string.IsNullOrWhiteSpace(value))
                    errors.Add($"Значение {fieldName} необходимо для добавления врача.");
                else if (value.Length > maxLength)
                    errors.Add($"Длина поля {fieldName} превышает {maxLength} символов.");
            }

            void ValidatePattern(string value, string fieldName, string pattern, string hint)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    errors.Add($"Поле {fieldName} необходимо для добавления пациента.");
                    return;
                }

                if (!Regex.IsMatch(value, pattern))
                    errors.Add($"Значение {fieldName} имеет неверный формат. Ожидание: {hint}.");
            }

            ValidateString(firstName, nameof(FirstName), 100);
            ValidateString(lastName, nameof(LastName), 100);
            ValidateString(address, nameof(Address), 500);
            ValidateString(disabilities, nameof(Disabilities), 1000);

            ValidatePattern(passportInfo, nameof(PassportInfo), @"^\d{10}$", "10 цифр: 4 цифры серия и 6 номер");
            ValidatePattern(snils, nameof(SNILS), @"^\d{11}$", "11 цифр (в учёт не идут пробелы и дефисы)");
            ValidatePattern(polis, nameof(Polis), @"^\d{16}$", "16 цифр (в учёт не идут пробелы)");
            ValidatePattern(phoneNumber, nameof(PhoneNumber), @"^[78]\d{10}$", "11 цифр, начинающихся с 7 или 8 (не с +)");

            if (errors.Count > 0)
                return (errors, null);

            var disease = new Patient(
                Guid.NewGuid(), firstName, 
                lastName, birthDay, address, 
                disabilities, passportInfo, snils, 
                polis, phoneNumber, lastVisit, nextVisit,
                doctorId, diseaseId);

            return (new List<string>(), disease);
        }

        public static Patient Restore(
            Guid id,
            string firstName,
            string lastName,
            DateTime birthDay,
            string address,
            string disabilities,
            string passportInfo,
            string snils,
            string polis,
            string phoneNumber,
            DateTime lastVisit,
            DateTime nextVisit,
            Guid? doctorId,
            Guid? diseaseId)
        {
            return new Patient(
                id, firstName, lastName, birthDay, address,
                disabilities, passportInfo, snils, polis,
                phoneNumber, lastVisit, nextVisit, doctorId, diseaseId);
        }

        // Методы для более глубокой связи domain model с логикой работы приложения, сейчас реализованы более простые связи

        //public void RemoveDoctor() => DoctorId = null;

        //public void RemoveDisease() => DiseaseId = null;

        //public void AssignDisease(Guid diseaseId)
        //{
        //    if (diseaseId == Guid.Empty)
        //        throw new ArgumentException("Id болезни не может быть пустым", nameof(diseaseId));

        //    DiseaseId = diseaseId;
        //}

        //public void AssignDoctor(Guid doctorId)
        //{
        //    if (doctorId == Guid.Empty)
        //        throw new ArgumentException("Id лечащего врача не может быть пустым", nameof(doctorId));

        //    DoctorId = doctorId;
        //}
    }
}
