using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HealthyAus.Models
{
    public class CONTACTTRACING
    {
        [Key]
        public int id { get; set; } 
        public string username1 { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string  username2 { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime encounter_date { get; set; }

        [Required(AllowEmptyStrings =true)]
        public string comment { get; set; }
        public int notify_status { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string notification_msg { get; set; }
    }
}
