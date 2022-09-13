using ProductLibrary.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductLibrary.API.ValidationAttributes
{
    public class ProductTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, 
            ValidationContext validationContext)
        {
            var product = (ProductForManipulationDto)validationContext.ObjectInstance;

            if (product.Title == product.Description)
            {
                return new ValidationResult(ErrorMessage,
                    new[] { nameof(ProductForManipulationDto) });
            }

            return ValidationResult.Success;
        }
    }
}
