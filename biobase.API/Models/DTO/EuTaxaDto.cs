namespace biobase.API.Models.DTO
{
    public class EuTaxaDto
    {
        public string EuSpeciesCode { get; set; }
        public int TaxonId { get; set; }
        public string? VernacularName { get; set; }
        public string ScientificName { get; set; }
        public string ScientificNameArt17 { get; set; }
        public string ScientificNameHdBd { get; set; }
        public string Directive { get; set; }
        public string? AnnexII { get; set; }
        public string? AnnexIV { get; set; }
        public string? AnnexV { get; set; }
        public string? Priority { get; set; }
        public string TaxaGroup { get; set; }
        public string Occurrence { get; set; }
        public string? Season { get; set; }
        public string NdffIdentity { get; set; }
    }
}
