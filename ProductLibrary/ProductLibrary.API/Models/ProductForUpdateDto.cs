using ProductLibrary.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductLibrary.API.Models
{
    public class ProductForUpdateDto : ProductForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out a description.")]
        public override string Description { get => base.Description; set => base.Description = value; }

    }
}
