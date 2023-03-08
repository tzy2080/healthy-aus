using System.ComponentModel.DataAnnotations;

namespace HealthyAus.Models
{
    public class STI_Protect
    {
        [Key]
        public int sti_id { get; set; }
        public int protect_id { get; set; }
    }
}
