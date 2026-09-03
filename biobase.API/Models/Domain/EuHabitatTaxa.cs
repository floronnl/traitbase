namespace biobase.API.Models.Domain
{
    public class EuHabitatTaxa
    {
        public string habitat_code { get; set; }
        public string? nednaam { get; set; }
        public string wetnaam { get; set; }
        public string taxa_group { get; set; }
        public string? category { get; set; }
        public string identity { get; set; }
    }
}
