using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardizedDate
{
    public class StandardizedDateHelper
    {
        public string ConvertToStandardFormat(string dateInput)
        {
            if (string.IsNullOrWhiteSpace(dateInput))
                throw new ArgumentException("Date input cannot be null or empty.");

            // Remove extra spaces and standardize the input
            dateInput = dateInput.Trim();

            // Define custom possible date formats
            string[] customFormats = {
                "dd/MM/yyyy",       // 25/01/2025
                "d MMMM yyyy",      // 25 January 2025
                "d. MMMM yyyy",     // 25. January 2025
                "dd.MM.yy",         // 25.01.25
                "dd-MM-yy",         // 25-01-25
                "d/MM/yyyy",        // 5/01/2025
                "MMMM yyyy",        // January 2025
                "dd MMM yyyy",      // 25 Jan 2025
                "d/MM/yy",          // 5/01/25
                "d MM yyyy",        // 5 01 2025
                "yyyy-MM-dd",       // 2025-01-25
                "yyyy/MM/dd",       // 2025/01/25
                "MM/dd/yyyy",       // 01/25/2025
                "MMMM d, yyyy",     // January 25, 2025
                "d MMM yy",         // 25 Jan 25
                "dd-MMMM-yyyy",     // 25-January-2025
                "MM-dd-yyyy"        // 01-25-2025
            };

            DateTime parsedDate;

            // Attempt to parse with custom formats
            if (DateTime.TryParseExact(dateInput, customFormats, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out parsedDate))
            {
                return parsedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            // Attempt a general parse
            if (DateTime.TryParse(dateInput, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out parsedDate))
            {
                return parsedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            // If no parsing succeeded, throw an error
            throw new FormatException($"Unable to parse date: {dateInput}");
        }
    }
}
