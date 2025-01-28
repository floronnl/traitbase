using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace biobase.API.Services
{
    public class CsvExportService : ICsvExportService
    {
        private const string delimiter = ";"; // Specify delimiter
        // Existing generic method for strongly typed classes
        public async Task<byte[]> ExportToCsvAsync<T>(List<T> data) where T : class
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, new UTF8Encoding(true)); // UTF-8 with BOM
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter // Set the custom delimiter
            });

            // Write the records to the CSV
            await csv.WriteRecordsAsync(data);
            await writer.FlushAsync();

            return memoryStream.ToArray();
        }

        // Overloaded method for dynamic data (List<IDictionary<string, object>>)
        public async Task<byte[]> ExportToCsvAsync(List<IDictionary<string, object>> data)
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, new UTF8Encoding(true));
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter // Set the custom delimiter
            });

            if (data.Count > 0)
            {
                // Write headers
                foreach (var key in data[0].Keys)
                {
                    csv.WriteField(key);
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
    }
}
