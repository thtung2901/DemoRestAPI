using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductLibrary.API.Models
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public int Age { get; set; }
        public string MainCategory { get; set; }
    }
}
