using System.ComponentModel.DataAnnotations;
namespace HealthyAus.Models
{
    public class Sex
    {
        [Key]
        public int sex_id { get; set; }
        public string sex { get; set; }
    }
}
