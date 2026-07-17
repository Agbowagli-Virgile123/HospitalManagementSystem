namespace HospitalManagementSystem.Services
{
    public class ModelValidator
    {
        public static ValidationResult Validate<T>(T model, IEnumerable<string> properties, bool required = true, bool validateDefaultValues = false)
        {
            var result = new ValidationResult();

            if (model == null)
            {
                result.Errors.Add("Model cannot be null.");
                return result;
            }

            var type = typeof(T);

            // Properties we will validate
            var propertiesToValidate = new List<string>();

            if (required)
            {
                // The provided list are the required properties
                propertiesToValidate = properties.ToList();
            }
            else
            {
                // The provided list are NOT required,
                // so validate every other property on the model
                var notRequired = properties.ToHashSet(StringComparer.OrdinalIgnoreCase);

                propertiesToValidate = type
                    .GetProperties()
                    .Select(x => x.Name)
                    .Where(x => !notRequired.Contains(x))
                    .ToList();
            }

            foreach (var propertyName in propertiesToValidate)
            {
                var property = type.GetProperty(propertyName);

                if (property == null)
                {
                    result.Errors.Add($"{propertyName} does not exist.");
                    continue;
                }

                var value = property.GetValue(model);

                if (value == null)
                {
                    result.Errors.Add($"{propertyName} is required.");
                    continue;
                }

                if (value is string s && string.IsNullOrWhiteSpace(s))
                {
                    result.Errors.Add($"{propertyName} is required.");
                    continue;
                }

                // Optionally validate default values for value types
                if (validateDefaultValues)
                {
                    var propertyType = Nullable.GetUnderlyingType(property.PropertyType)
                                      ?? property.PropertyType;

                    if (propertyType.IsValueType)
                    {
                        var defaultValue = Activator.CreateInstance(propertyType);

                        if (Equals(value, defaultValue))
                        {
                            result.Errors.Add($"{property.Name} cannot have the default value.");
                        }
                    }
                }
            }

            return result;
        }
    }

    public class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<string> Errors { get; set; } = new();
    }
}