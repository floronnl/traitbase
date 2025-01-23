using System.ComponentModel.DataAnnotations;

namespace biobase.API.Models.Domain
{
    public class User
    {
        [Key]
        public int usrid { get; set; }

        [Required]
        [MaxLength(200)] // Adjust length as needed based on your API key format
        public string apikey { get; set; }

        public string authorization { get; set; }
    }
}