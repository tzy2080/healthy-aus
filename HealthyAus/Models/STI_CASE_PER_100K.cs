using System.ComponentModel.DataAnnotations;
namespace HealthyAus.Models
{
    public class STI_CASE_PER_100K
    {
        [Key]
        public int case_id { get; set; }
        public int sti_id { get; set; }
        public int year_id { get; set; }
        public int age_id { get; set; }
        public int loc_id { get; set; }
        public int sex_id { get; set; }
        public int remote_id { get; set; }
        public decimal rate { get; set; }


    }
}
