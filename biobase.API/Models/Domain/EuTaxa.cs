namespace biobase.API.Models.Domain
{
    public class EuTaxa
    {
        public string speciescode { get; set; }
        public string taxon_id { get; set; }
        public string nednaam { get; set; }
        public string wetnaam { get; set; }
        public string name_art17 { get; set; }
        public string name_hd_bd { get; set; }
        public string directive {  get; set; }
        public string? annex_ii { get; set; }
        public string? annex_iv { get; set; }
        public string? annex_v { get; set; }
        public string? priority { get; set; }
        public string taxa_group { get; set; }
        public string occurrence { get; set; }
        public string? season { get; set; }
        public string ndff_uri { get; set; }
    }
}
