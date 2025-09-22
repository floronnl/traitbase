
namespace biobase.API.Models.DTO
{
    public class TaxaDto
    {
        public int TaxonId { get; set; }
        public string? TaxaGroup { get; set; }
        public string? ScientificName { get; set; }
        public string? VernacularName { get; set; }
        public string? Family { get; set; }
        public string? TaxonLabel { get; set; }
        public string? NdffIdentity { get; set; }
        public string? ThreatStatus { get; set; }
        public string? OccurenceStatus { get; set; }
        public string? TaxaSubgroup{ get; set; }
        public string? IsAcceptedName {  get; set; }
        public string? ScientificNameAuthorship { get; set; }
        public DateTime Updated { get; set; }
    }
}