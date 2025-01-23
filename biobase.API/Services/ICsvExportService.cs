namespace biobase.API.Services
{
    public interface ICsvExportService
    {
        Task<byte[]> ExportToCsvAsync<T>(List<T> data) where T : class;
        Task<byte[]> ExportToCsvAsync(List<IDictionary<string, object>> data);
    }
}
