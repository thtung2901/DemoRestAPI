using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductLibrary.API.Models
{
    public class CategoryForCreationDto
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public string MainCategory { get; set; }
        public ICollection<ProductForCreationDto> Products { get; set; }
          = new List<ProductForCreationDto>();

    }
}
