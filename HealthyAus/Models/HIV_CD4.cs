using System.ComponentModel.DataAnnotations;

namespace HealthyAus.Models
{
    public class HIV_CD4
    {
        [Key]
        public int person_id { get; set; }
        public int loc_id { get; set; }
        public int sex_id { get; set; }
        public int age_id { get; set; }
        public int exp_id { get; set; }
        public int year_neg_diag_id { get; set; }
        public int year_pos_diag_id { get; set; }
        public int diag_delay { get; set; }
    }
}
