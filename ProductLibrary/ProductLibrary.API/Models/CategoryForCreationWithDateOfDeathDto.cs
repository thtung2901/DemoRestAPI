using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductLibrary.API.Models
{
    public class CategoryForCreationWithDateDeletedDto : CategoryForCreationDto
    {
        public DateTimeOffset? DateDeleted { get; set; }
    }
}
