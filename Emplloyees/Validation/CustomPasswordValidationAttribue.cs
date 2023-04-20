using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Emplloyees.Validation
{
    public class CustomPasswordValidationAttribue : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return false;
            string password = value.ToString();
            bool isValid = Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");

            return isValid;
        }
        public override string FormatErrorMessage(string name)
        {
            return $"The {name} must be 8 character including one capital and small letter with one number.";
        }
    }
}
