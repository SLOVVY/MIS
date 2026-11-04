namespace MIS.API.Models
{
    public class Disease
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Symptoms { get; }
        public string Medicaments { get; }
        public string Recommendations { get; }

        private Disease(
            Guid id, 
            string name, 
            string symptoms,
            string medicaments,
            string recommendations)
        {
            Id = id;
            Name = name;
            Symptoms = symptoms;
            Medicaments = medicaments;
            Recommendations = recommendations;
        }

        public static (List<string> errors, Disease? disease) CreateDisease(
            string name,
            string symptoms,
            string medicaments,
            string recommendations)
        {
            List<string> errors = [];

            void ValidateString(string value, string fieldName, int maxLength)
            {
                if (string.IsNullOrWhiteSpace(value))
                    errors.Add($"Значение {fieldName} необходимо для добавления болезни.");
                else if (value.Length > maxLength)
                    errors.Add($"Длина поля {fieldName} превышает {maxLength} символов.");
            }

            ValidateString(name, nameof(Name), 200);
            ValidateString(symptoms, nameof(Symptoms), 2000);
            ValidateString(medicaments, nameof(Medicaments), 2000);
            ValidateString(recommendations, nameof(Recommendations), 2000);

            if (errors.Count > 0)
                return (errors, null!);

            var disease = new Disease(Guid.NewGuid(), name, symptoms, medicaments, recommendations);
            return (new List<string>(), disease);
        }

        public static Disease Restore(
        Guid id,
        string name,
        string symptoms,
        string medicaments,
        string recommendations)
        {
            return new Disease(id, name, symptoms, medicaments, recommendations);
        }
    }
}
