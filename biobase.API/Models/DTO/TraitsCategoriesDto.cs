using System.ComponentModel.DataAnnotations;

namespace biobase.API.Models.DTO
{
    public class TraitsCategoriesDto
    {
        [Key]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? TaxaGroup { get; set; }
        public string? DataType { get; set; }
        public string? DefaultCopyright { get; set; }
        public string? Source { get; set; }
        public string? Description { get; set; }
    }
}
