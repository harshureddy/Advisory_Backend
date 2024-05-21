using System.ComponentModel.DataAnnotations;

namespace AdvisorAPI.Models
{
    public class Advisor
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required, MinLength(9), MaxLength(9)]
        public string SIN { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Address { get; set; } = string.Empty;

        [MinLength(8), MaxLength(8)]
        public string Phone { get; set; } =  string.Empty;

        [Required]
        public string HealthStatus { get; set; } = string.Empty;
    }
}
