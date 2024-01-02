using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HunterPie.UI.Architecture.Validation;

#nullable enable
public class EmailValidationRule : ValidationRule
{
    private const string EMAIL_FORMAT = @"(@)(.+)$";
    private const string EMAIL_PATTERN = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    private const string INVALID_EMAIL = "VALIDATION_FOR_EMAIL_FAIL";
    
    private static string DomainMapper(Match match)
    {
        var idnMapping = new IdnMapping();

        string domainName = idnMapping.GetAscii(match.Groups[2].Value);

        return match.Groups[1].Value + domainName;
    }
    
    public override ValidationResult? Validate(object value, CultureInfo cultureInfo)
    {
        if (value is not string email)
            throw new ArgumentException("value must be string");

        if (string.IsNullOrWhiteSpace(email))
            return new ValidationResult(INVALID_EMAIL);

        bool isValid = false;

        try
        {
            string normalizedEmail = Regex.Replace(email, EMAIL_FORMAT, DomainMapper, RegexOptions.None,
                TimeSpan.FromMilliseconds(200));

            isValid = Regex.IsMatch(normalizedEmail, EMAIL_PATTERN, RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(200));
        }
        catch
        {
            isValid = false;
        }
        
        return isValid ? ValidationResult.Success : new ValidationResult(INVALID_EMAIL);
    }
}
