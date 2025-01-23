using Dapper;
using MySqlConnector;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace biobase.API.Repositories
{
    public class SQLTraitsRepository : ITraitsRepository
    {
        private readonly string _connectionString;

        public SQLTraitsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IDictionary<string, object>> GetTraitsSingleAsync(int species_id)
        {
            if (species_id <= 0)
            {
                throw new ArgumentException("Invalid species ID");
            }

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Query the traits view directly
                    var query = @"
                        SELECT *
                        FROM biobase.traits
                        WHERE soortnummer = @species_id";

                    var traits = await connection.QueryAsync<(int Soortnummer, string Rubriek, string Weergave)>(
                        query, new { species_id });

                    if (!traits.Any())
                    {
                        return new Dictionary<string, object> { { "Error", $"No traits data found for species ID {species_id}" } };
                    }

                    // Perform pivoting in code
                    var pivotedResult = new Dictionary<string, object> { { "soortnummer", species_id } };
                    foreach (var trait in traits)
                    {
                        pivotedResult[trait.Rubriek] = trait.Weergave == null || trait.Weergave is DBNull
                            ? DBNull.Value
                            : trait.Weergave;
                    }
                    return pivotedResult;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTraitsSingleAsync: {ex.Message}");
                throw; // Consider logging or handling the exception appropriately
            }
        }
        public async Task<List<IDictionary<string, object>>> GetTraitsPivotAsync()
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                // Get all columns from pivot table
                var query = "SELECT * FROM traits_pivot";

                var result = await connection.QueryAsync(query);

                // Convert the result into a list of dictionaries for dynamic column handling
                return result.Select(row => (IDictionary<string, object>)row).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTraitsPivotAsync: {ex.Message}");
                throw;
            }
        }
    }
}