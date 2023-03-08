using System.ComponentModel.DataAnnotations;

namespace HealthyAus.Models
{
    public class LOCATION
    {
        [Key]
        public int loc_id { get; set; }
        public string loc { get; set; }
    }
}
