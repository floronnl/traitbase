using CsvHelper;
using System.Globalization;
using System.Text;

namespace biobase.API.Services
{
    public class CsvExportService : ICsvExportService
    {
        // Existing generic method for strongly typed classes
        public async Task<byte[]> ExportToCsvAsync<T>(List<T> data) where T : class
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            // Write the records to the CSV
            await csv.WriteRecordsAsync(data);
            await writer.FlushAsync();

            return memoryStream.ToArray();
        }

        // Overloaded method for dynamic data (List<IDictionary<string, object>>)
        public async Task<byte[]> ExportToCsvAsync(List<IDictionary<string, object>> data)
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

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
