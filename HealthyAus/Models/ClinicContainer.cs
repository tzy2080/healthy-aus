namespace HealthyAus.Models
{
    public class ClinicContainer
    {
        public string name { get; set; }
        public string address { get; set; }
        public List<string> services { get; set; }
        public List<decimal> coordinates { get; set; }
    }
}
