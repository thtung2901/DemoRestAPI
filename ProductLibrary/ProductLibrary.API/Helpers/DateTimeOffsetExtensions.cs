using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductLibrary.API.Helpers
{
    public static class DateTimeOffsetExtensions
    {
        public static int GetCurrentAge(this DateTimeOffset dateTimeOffset,
            DateTimeOffset? dateDeleted)
        {
            var dateToCalculateTo = DateTime.UtcNow;

            if (dateDeleted != null)
            {
                dateToCalculateTo = dateDeleted.Value.UtcDateTime;
            }

            var age = dateToCalculateTo.Year - dateTimeOffset.Year;

            if (dateToCalculateTo < dateTimeOffset.AddYears(age))
            {
                age--;
            }

            return age; 
        }
    }
}
