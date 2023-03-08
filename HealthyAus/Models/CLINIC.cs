using Microsoft.EntityFrameworkCore;

namespace HealthyAus.Models
{
    [Keyless]
    public class CLINIC
    {
        public string name { get; set; }
        public string address { get; set; }
        public string postcode { get; set; }
        public int free { get; set; }
        public int anonymous { get; set; }
        public int condom { get; set; }
        public decimal lat { get; set; }
        public decimal lon { get; set; }
    }
}
