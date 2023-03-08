using System.ComponentModel.DataAnnotations;

namespace HealthyAus.Models
{
    public class AGE_GRP
    {
        [Key]
        public int age_id { get; set; }
        public string age_grp { get; set; }
    }
}
