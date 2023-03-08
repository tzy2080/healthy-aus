using System.ComponentModel.DataAnnotations;
namespace HealthyAus.Models
{
    public class Remoteness
    {
        [Key]
        public int remote_id { get; set; }
        public string remoteness { get; set; }
    }
}
