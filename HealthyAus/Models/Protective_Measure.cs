using System.ComponentModel.DataAnnotations;

namespace HealthyAus.Models
{
    public class Protective_Measure
    {
        [Key]
        public int protect_id { get; set; }
        public string protect_meas { get; set; }
    }
}
