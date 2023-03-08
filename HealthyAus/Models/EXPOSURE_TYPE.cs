using System.ComponentModel.DataAnnotations;

namespace HealthyAus.Models
{
    public class EXPOSURE_TYPE
    {
        [Key]
        public int exp_id { get; set; }
        public string exp_category { get; set; }
    }
}
