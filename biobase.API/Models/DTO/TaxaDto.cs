
namespace biobase.API.Models.DTO
{
    public class TaxaDto
    {
        public int species_id { get; set; }
        public string? taxon_class { get; set; }
        public string? sci_name { get; set; }
        public string? nl_name { get; set; }
        public string? family { get; set; }
        public string? label { get; set; }
        public string? identity { get; set; }
        public string? red_list_status { get; set; }
        public string? rareness { get; set; }
        public string? taxon_subclass{ get; set; }
        public string? acc {  get; set; }
        public string? author { get; set; }
        public DateTime updated { get; set; }
    }
}