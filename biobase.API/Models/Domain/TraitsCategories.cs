using System.ComponentModel.DataAnnotations;

namespace biobase.API.Models.Domain
{
    public class TraitsCategories
    {
        [Key]
        public int rubriekid { get; set; }
        public string? rubriek { get; set; }
        public string? soortgroep { get; set; }
        public string? datatype { get; set; }
        public string? default_copyright { get; set; }
        public string? bron { get; set; }
        public string? autorisatie { get; set; }
        public string? description { get; set; }
        public string? description_short { get; set; }
        public string? project { get; set; }
        public bool? project_order { get; set; }

    }
}
