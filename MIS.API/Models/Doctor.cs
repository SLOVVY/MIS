namespace MIS.API.Models
{
    public class Doctor
    {
        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Specialization { get; }
        public int Experience { get; }
        public int CabinetNumber { get; }

        private Doctor(
            Guid id,
            string firstName,
            string lastName,
            string specialization,
            int experience,
            int cabinetNumber)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Specialization = specialization;
            Experience = experience;
            CabinetNumber = cabinetNumber;
        }

        public static (List<string> errors, Doctor? doctor) CreateDoctor(
            string firstName,
            string lastName,
            string specialization,
            int experience,
            int cabinetNumber)
        {
            List<string> errors = [];

            void ValidateString(string value, string fieldName, int maxLength)
            {
                if (string.IsNullOrWhiteSpace(value))
                    errors.Add($"Значение {fieldName} необходимо для добавления врача.");
                else if (value.Length > maxLength)
                    errors.Add($"Длина поля {fieldName} превышает {maxLength} символов.");
            }

            ValidateString(firstName, nameof(FirstName), 100);
            ValidateString(lastName, nameof(LastName), 100);
            ValidateString(specialization, nameof(Specialization), 500);

            if (experience > 100) errors.Add("Проверьте корректность введённых лет стажа.");

            if (errors.Count > 0)
                return (errors, null!);

            var disease = new Doctor(Guid.NewGuid(), firstName, lastName, specialization, experience, cabinetNumber);
            return (new List<string>(), disease);
        }

        public static Doctor Restore(
            Guid id,
            string firstName,
            string lastName,
            string specialization,
            int experience,
            int cabinetNumber)
        {
            return new Doctor(id, firstName, lastName, specialization, experience, cabinetNumber);
        }
    }
}
