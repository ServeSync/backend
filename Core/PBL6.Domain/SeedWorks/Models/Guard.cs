using PBL6.Domain.SeedWorks.Exceptions.Resources;

namespace PBL6.Domain.SeedWorks.Models;

public static class Guard
{
    public static T NotNull<T>(
        T? value,
        string parameterName)
    {
        if (value == null)
        {
            throw new ResourceInvalidDataException($"{parameterName} can not be null!");
        }

        return value;
    }
    
    public static T NotNull<T>(
        T? value,
        string parameterName,
        string message)
    {
        if (value == null)
        {
            throw new ResourceInvalidDataException($"{parameterName} can not be null!");
        }

        return value;
    }
    
    public static string NotNull(
        string? value,
        string parameterName,
        int maxLength = int.MaxValue,
        int minLength = 0)
    {
        if (value == null)
        {
            throw new ResourceInvalidDataException($"{parameterName} can not be null!");
        }

        if (value.Length > maxLength)
        {
            throw new ResourceInvalidDataException($"{parameterName} length must be equal to or lower than {maxLength}!");
        }

        if (minLength > 0 && value.Length < minLength)
        {
            throw new ResourceInvalidDataException($"{parameterName} length must be equal to or bigger than {minLength}!");
        }

        return value;
    }
    
    public static string NotNullOrWhiteSpace(
        string? value,
        string parameterName,
        int maxLength = int.MaxValue,
        int minLength = 0)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ResourceInvalidDataException($"{parameterName} can not be null, empty or white space!");
        }

        if (value!.Length > maxLength)
        {
            throw new ResourceInvalidDataException($"{parameterName} length must be equal to or lower than {maxLength}!");
        }

        if (minLength > 0 && value!.Length < minLength)
        {
            throw new ResourceInvalidDataException($"{parameterName} length must be equal to or bigger than {minLength}!");
        }

        return value;
    }
    
    public static string NotNullOrEmpty(
        string? value,
        string parameterName,
        int maxLength = int.MaxValue,
        int minLength = 0)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ResourceInvalidDataException($"{parameterName} can not be null or empty!");
        }

        if (value!.Length > maxLength)
        {
            throw new ResourceInvalidDataException($"{parameterName} length must be equal to or lower than {maxLength}!");
        }

        if (minLength > 0 && value!.Length < minLength)
        {
            throw new ResourceInvalidDataException($"{parameterName} length must be equal to or bigger than {minLength}!");
        }

        return value;
    }
    
    public static ICollection<T> NotNullOrEmpty<T>(ICollection<T>? value, string parameterName)
    {
        if (value == null || value.Count <= 0)
        {
            throw new ResourceInvalidDataException(parameterName + " can not be null or empty!");
        }

        return value;
    }

    public static string? Length(
        string? value,
        string parameterName,
        int maxLength,
        int minLength = 0)
    {
        if (minLength > 0)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ResourceInvalidDataException(parameterName + " can not be null or empty!");
            }

            if (value!.Length < minLength)
            {
                throw new ResourceInvalidDataException($"{parameterName} length must be equal to or bigger than {minLength}!");
            }
        }

        if (value != null && value.Length > maxLength)
        {
            throw new ResourceInvalidDataException($"{parameterName} length must be equal to or lower than {maxLength}!");
        }

        return value;
    }

    public static int Positive(
        int value,
        string parameterName)
    {
        if (value == 0)
        {
            throw new ResourceInvalidDataException($"{parameterName} is equal to zero");
        }
        else if (value < 0)
        {
            throw new ResourceInvalidDataException($"{parameterName} is less than zero");
        }
        return value;
    }
    
    public static float Positive(
        float value,
        string parameterName)
    {
        if (value == 0)
        {
            throw new ResourceInvalidDataException($"{parameterName} is equal to zero");
        }
        else if (value < 0)
        {
            throw new ResourceInvalidDataException($"{parameterName} is less than zero");
        }
        return value;
    }

    public static double Positive(
        double value,
        string parameterName)
    {
        if (value == 0)
        {
            throw new ResourceInvalidDataException($"{parameterName} is equal to zero");
        }
        else if (value < 0)
        {
            throw new ResourceInvalidDataException($"{parameterName} is less than zero");
        }
        return value;
    }

    public static decimal Positive(
        decimal value,
        string parameterName)
    {
        if (value == 0)
        {
            throw new ResourceInvalidDataException($"{parameterName} is equal to zero");
        }
        else if (value < 0)
        {
            throw new ResourceInvalidDataException($"{parameterName} is less than zero");
        }
        return value;
    }

    public static int Range(
        int value,
        string parameterName,
        int minimumValue,
        int maximumValue = int.MaxValue)
    {
        if (value < minimumValue || value > maximumValue)
        {
            throw new ResourceInvalidDataException($"{parameterName} is out of range min: {minimumValue} - max: {maximumValue}");
        }

        return value;
    }
    
    public static float Range(
        float value,
        string parameterName,
        float minimumValue,
        float maximumValue = float.MaxValue)
    {
        if (value < minimumValue || value > maximumValue)
        {
            throw new ResourceInvalidDataException($"{parameterName} is out of range min: {minimumValue} - max: {maximumValue}");
        }
        return value;
    }


    public static double Range(
        double value,
        string parameterName,
        double minimumValue,
        double maximumValue = double.MaxValue)
    {
        if (value < minimumValue || value > maximumValue)
        {
            throw new ResourceInvalidDataException($"{parameterName} is out of range min: {minimumValue} - max: {maximumValue}");
        }

        return value;
    }


    public static decimal Range(
        decimal value,
        string parameterName,
        decimal minimumValue,
        decimal maximumValue = decimal.MaxValue)
    {
        if (value < minimumValue || value > maximumValue)
        {
            throw new ResourceInvalidDataException($"{parameterName} is out of range min: {minimumValue} - max: {maximumValue}");
        }

        return value;
    }

    public static T NotDefaultOrNull<T>(
        T? value,
        string parameterName)
        where T : struct
    {
        if (value == null)
        {
            throw new ResourceInvalidDataException($"{parameterName} is null!");
        }

        if (value.Value.Equals(default(T)))
        {
            throw new ResourceInvalidDataException($"{parameterName} has a default value!");
        }

        return value.Value;
    }
}