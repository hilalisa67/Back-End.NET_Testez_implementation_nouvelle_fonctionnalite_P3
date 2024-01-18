using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class IsNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string strValue && !double.TryParse(strValue, out _))
            {
                return new ValidationResult("PriceNotANumber");
            }

            return ValidationResult.Success;
        }
    }
}