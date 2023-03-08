using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HealthyAus.Models
{
    public class Stats
    {
        [Key]
        public int Id { get; set; }
    }
}
