using System.ComponentModel.DataAnnotations;

namespace HealthyAus.Models
{
    public class STI_Symptom
    {
        [Key]
        public int sti_id { get; set; }
        public int symptom_id { get; set; }
    }
}
