using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HealthyAus.Models
{
    public class Symptom
    {
        [Key]
        public int symptom_id { get; set; }
        public string symptom { get; set; }
    }
}
