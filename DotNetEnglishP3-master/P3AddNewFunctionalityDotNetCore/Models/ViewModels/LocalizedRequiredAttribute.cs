using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels;

public class LocalizedRequiredAttribute<TResource> : RequiredAttribute
{
    public LocalizedRequiredAttribute(string errorMessage)
    {
        AllowEmptyStrings = false;
        ErrorMessageResourceName = errorMessage;
        ErrorMessageResourceType = typeof(TResource);
    }
}