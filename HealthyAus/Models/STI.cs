using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HealthyAus.Models
{
    public class STI
    {
        [Key]
        public int sti_id { get; set; }
        public string sti_name { get; set; }
    }
}
