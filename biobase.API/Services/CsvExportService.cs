using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using System.Reflection;

namespace biobase.API.Services
{
    public class CsvExportService : ICsvExportService
    {
        private const string Delimiter = ";"; // Specify delimiter

        // Generic method for strongly typed classes
        public async Task<byte[]> ExportToCsvAsync<T>(List<T> data) where T : class
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, new UTF8Encoding(true));
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = Delimiter
            });

            if (data.Count > 0)
            {
                // Write camelCase headers
                WriteHeaders(csv, typeof(T));

                // Write records
                foreach (var record in data)
                {
                    foreach (var prop in typeof(T).GetProperties())
                    {
                        csv.WriteField(prop.GetValue(record));
                    }
                    csv.NextRecord();
                }
            }

            await writer.FlushAsync();
            return memoryStream.ToArray();
        }

        // Overloaded method for dynamic dictionaries
        public async Task<byte[]> ExportToCsvAsync(List<IDictionary<string, object>> data)
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, new UTF8Encoding(true));
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = Delimiter
            });

            if (data.Count > 0)
            {
                // Write camelCase headers
                foreach (var key in data[0].Keys)
                {
                    csv.WriteField(ToCamelCase(key));
                }
                csv.NextRecord();

                // Write rows
                foreach (var row in data)
                {
                    foreach (var value in row.Values)
                    {
                        csv.WriteField(value);
                    }
                    csv.NextRecord();
                }
            }

            await writer.FlushAsync();
            return memoryStream.ToArray();
        }

        // Helper: Write headers for typed objects
        private void WriteHeaders(CsvWriter csv, Type type)
        {
            foreach (var prop in type.GetProperties())
            {
                csv.WriteField(ToCamelCase(prop.Name));
            }
            csv.NextRecord();
        }

        // Helper: Convert PascalCase to camelCase
        private static string ToCamelCase(string value)
        {
            if (string.IsNullOrEmpty(value) || !char.IsUpper(value[0]))
                return value;

            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }
    }
}
