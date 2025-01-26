using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

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

            // Normalize month names for French (add more cultures if needed)
            dateInput = NormalizeMonthNames(dateInput);

            // List of cultures to try (in order of preference)
            var cultures = new List<CultureInfo>
            {
                CultureInfo.InvariantCulture, // English (default)
                new CultureInfo("fr-FR"),     // French
                // Add more cultures here as needed
            };

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
                "MM-dd-yyyy",        // 01-25-2025
                "dddd dd MMM yyyy",   // sunday 26 jan 2024
                "dddd dd MMMM yyyy",  // sunday 26 january 2024
            };

            DateTime parsedDate;

            // Try parsing with each culture
            foreach (var culture in cultures)
            {
                // Attempt to parse with custom formats
                if (DateTime.TryParseExact(dateInput, customFormats, culture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal, out parsedDate))
                {
                    return parsedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                // Attempt a general parse
                if (DateTime.TryParse(dateInput, culture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal, out parsedDate))
                {
                    return parsedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
            }

            // If no parsing succeeded, throw an error
            throw new FormatException($"Unable to parse date: {dateInput}");
        }

        // Helper method to normalize input (remove accents, convert to lowercase)
        private string NormalizeInput(string input)
        {
            // Remove diacritics (accents) from characters
            string normalized = input.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            // Return cleaned input (lowercase)
            return builder.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
        }

        // Helper method to normalize month names for specific cultures
        private string NormalizeMonthNames(string input)
        {
            // Define mapping for French months (non-accented → accented)
            var frenchMonths = new Dictionary<string, string>
            {
                { "janvier", "janvier" },
                { "fevrier", "février" },
                { "mars", "mars" },
                { "avril", "avril" },
                { "mai", "mai" },
                { "juin", "juin" },
                { "juillet", "juillet" },
                { "aout", "août" },
                { "septembre", "septembre" },
                { "octobre", "octobre" },
                { "novembre", "novembre" },
                { "decembre", "décembre" }
            };

            foreach (var month in frenchMonths)
            {
                input = Regex.Replace(input, $@"\b{month.Key}\b", month.Value, RegexOptions.IgnoreCase);
            }

            return input;
        }

    }
}
