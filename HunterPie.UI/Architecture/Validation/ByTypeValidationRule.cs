using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HunterPie.UI.Architecture.Validation;

public class ByTypeValidationRule : ValidationRule
{
    public Type ValidationType { get; set; }

    public override ValidationResult? Validate(object value, CultureInfo cultureInfo)
    {
        string strValue = Convert.ToString(value);

        return string.IsNullOrEmpty(strValue)
            ? new ValidationResult("Value cannot be converted to string.")
            : ValidationType.Name switch
            {
                "Boolean" => bool.TryParse(strValue, out _) ? ValidationResult.Success : new ValidationResult("Input should be type of boolean"),
                "Int32" => int.TryParse(strValue, out _) ? ValidationResult.Success : new ValidationResult("Input should be type of Int32"),
                "Double" => double.TryParse(strValue, NumberStyles.Float, CultureInfo.InvariantCulture, out _) ? ValidationResult.Success : new ValidationResult("Input should be type of Double"),
                "Int64" => long.TryParse(strValue, out _) ? ValidationResult.Success : new ValidationResult("Input should be type of Int64"),
                _ => throw new InvalidCastException($"{ValidationType.Name} is not supported"),
            };
    }
}
