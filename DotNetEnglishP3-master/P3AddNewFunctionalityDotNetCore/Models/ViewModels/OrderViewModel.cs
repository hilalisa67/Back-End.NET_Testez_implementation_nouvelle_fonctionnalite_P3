using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using P3AddNewFunctionalityDotNetCore.Resources.Models.ViewModels;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class OrderViewModel
    {
        [BindNever]
        public int OrderId { get; init; }

        [BindNever]
        public ICollection<CartLine> Lines { get; set; }
        
        [LocalizedRequired<Order>("ErrorMissingName")]
        public string Name { get; init; }

        [LocalizedRequired<Order>("ErrorMissingAddress")]
        public string Address { get; init; }

        [LocalizedRequired<Order>("ErrorMissingCity")]
        public string City { get; init; }

        [LocalizedRequired<Order>("ErrorMissingZipCode")]
        public string Zip { get; init; }

        [LocalizedRequired<Order>("ErrorMissingCountry")]
        public string Country { get; init; }

        [BindNever]
        public DateTime Date { get; init; }
    }
}
