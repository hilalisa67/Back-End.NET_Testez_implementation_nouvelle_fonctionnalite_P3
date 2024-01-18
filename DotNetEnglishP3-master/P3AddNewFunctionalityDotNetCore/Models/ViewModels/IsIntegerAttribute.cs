using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class IsIntegerAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string strValue && !int.TryParse(strValue, out _))
            {
                return new ValidationResult("QuantityNotAnInteger");
            }

            return ValidationResult.Success;
        }
    }
}