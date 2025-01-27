using System.ComponentModel.DataAnnotations;

namespace biobase.API.Models.Domain
{
    public class Taxa
    {
        [Key]
        public int soortnummer { get; set; }
        public string? groep { get; set; }
        public string? wetnaam { get; set; }
        public string? nednaam { get; set; }
        public string? familie { get; set; }
        public string? label { get; set; }
        public string? identity { get; set; }
        public string? rl { get; set; }
        public string? zzz { get; set; }
        public string? deelgroep { get; set; }
        public string? soortcode { get; set; }
        public int? acc { get; set; }
        public string? german { get; set; }
        public string? jaar { get; set; }
        public string? auteur { get; set; }
        public string? english { get; set; }
        public int? niet_publiek { get; set; }
        public string? deleted { get; set; }
        public DateTime updated { get; set; }
    }
}