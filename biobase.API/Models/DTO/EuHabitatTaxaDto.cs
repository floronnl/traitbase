namespace biobase.API.Models.DTO
{
    public class EuHabitatTaxaDto
    {
        public string HabitatCode { get; set; }
        public string? VernacularName { get; set; }
        public string ScientificName { get; set; }
        public string TaxaGroup { get; set; }
        public string? Category { get; set; }
        public string NdffIdentity { get; set; }
    }
}
