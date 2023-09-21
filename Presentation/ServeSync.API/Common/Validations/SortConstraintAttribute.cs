using System.ComponentModel.DataAnnotations;

namespace ServeSync.API.Common.Validations;

public class SortConstraintAttribute : ValidationAttribute
{
    public string Fields { get; set; }

    protected override ValidationResult IsValid(object? data, ValidationContext validationContext)
    {
        if (IsValidSortingData(data, Fields))
        {
            var sortingQueryValue = data.ToString().ToLower();
            var values = sortingQueryValue.Split(new char[] { ',' }, StringSplitOptions.TrimEntries);
            var allowedSortingFields = Fields.ToLower().Split(new char[] { ',' }, StringSplitOptions.TrimEntries);

            foreach (var value in values)
            {
                var splitedValue = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (!allowedSortingFields.Contains(splitedValue[0]))
                {
                    return new ValidationResult($"Only sorting in '{Fields}' fields is allowed!");
                }

                if (splitedValue.Length > 1 && !IsValidSortingOrder(splitedValue[1]))
                {
                    return new ValidationResult($"Orderby param only accepts 'asc' or 'desc'!");
                }
            }
        }

        return ValidationResult.Success;
    }

    private bool IsValidSortingData(object data, string fields)
    {
        return !string.IsNullOrWhiteSpace(fields) && !string.IsNullOrWhiteSpace(data.ToString());
    }

    private bool IsValidSortingOrder(string order)
    {
        return order == "asc" || order == "desc";
    }
}