namespace HealthyAus.Models
{
    public class STICaseViewModel
    {
        public STICaseViewModel()
        {
            STICaseContainers = new List<STICaseContainer>();
        }
        public List<STICaseContainer> STICaseContainers { get; set; }
    }
}
