using System.ComponentModel.DataAnnotations;

namespace biobase.API.Models.DTO
{
    public class TraitsCategoriesDto
    {
        [Key]
        public int category_id { get; set; }
        public string? category_name { get; set; }
        public string? taxon_class { get; set; }
        public string? datatype { get; set; }
        public string? default_copyright { get; set; }
        public string? source { get; set; }
        public string? description { get; set; }
    }
}
