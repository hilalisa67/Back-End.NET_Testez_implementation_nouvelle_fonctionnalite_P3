using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using Ressource;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever] public int Id { get; init; }

        [LocalizedRequired<ProductService>("MissingName")]
        public string Name { get; init; }

        [LocalizedRequired<ProductService>("MissingDescription")]
        public string Description { get; init; }

        [LocalizedRequired<ProductService>("MissingDetails")]
        public string Details { get; init; }

        [LocalizedRequired<ProductService>("MissingStock")]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "StockNotGreaterThanZero",
            ErrorMessageResourceType = typeof(Ressource.ProductService))]
        [RegularExpression(@"^\d+$", ErrorMessageResourceName = "StockNotAnInteger",
            ErrorMessageResourceType = typeof(Ressource.ProductService))]
        public string Stock { get; init; }

        [LocalizedRequired<ProductService>("MissingPrice")]
        [RegularExpression(@"^[0-9]*([.,][0-9]+)?$", ErrorMessageResourceName = "PriceNotANumber", ErrorMessageResourceType = typeof(Ressource.ProductService))]
        [Range(0.01, double.MaxValue, ErrorMessageResourceName = "PriceNotGreaterThanZero", ErrorMessageResourceType = typeof(Ressource.ProductService))]
        public string Price { get; init; }
    }
}