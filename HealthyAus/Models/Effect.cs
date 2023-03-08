using System.ComponentModel.DataAnnotations;

namespace HealthyAus.Models
{
    public class Effect
    {
        [Key]
        public int effect_id { get; set; }
        public string effect { get; set; }
    }
}
