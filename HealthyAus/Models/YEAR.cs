using System.ComponentModel.DataAnnotations;

namespace HealthyAus.Models
{
    public class YEAR
    {
        [Key]
        public int year_id { get; set; }
        public int year { get; set; }
    }
}
