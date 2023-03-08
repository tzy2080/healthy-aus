namespace HealthyAus.Models
{
    public class STIViewModel
    {
        public STIViewModel()
        {
            STIContainers = new List<STIContainer>();
        }
        public List<STIContainer> STIContainers { get; set; }
    }
}
