using Microsoft.AspNetCore.Mvc;
using HealthyAus.Models;

namespace HealthyAus.Controllers
{
    public class TestServicesFreeCondoms : Controller
    {
        private readonly ILogger<TestServicesFreeCondoms> _logger;
        private readonly STDContext _stdContext;

        public TestServicesFreeCondoms(ILogger<TestServicesFreeCondoms> logger, STDContext sTDContext)
        {
            _logger = logger;
            _stdContext = sTDContext;
        }
        public IActionResult Index()
        {
            //retrieve all clinic info
            var clinics = from clinic in _stdContext.CLINIC
                          select clinic;
            
            //put data in desired format to be passed to View
            var lst = new List<ClinicContainer>();
            foreach(var clinic in clinics)
            {
                //convert integer indicator of service availability into list of string
                var svc = new List<string>();
                if (clinic.free == 1)
                {
                    svc.Add("free_testing");
                }
                if (clinic.anonymous == 1)
                {
                    svc.Add("anonymous_testing");
                }
                if (clinic.condom == 1)
                {
                    svc.Add("free_condom");
                }
                //get coordinates
                var coordi = new List<decimal>();
                coordi.Add(clinic.lon);
                coordi.Add(clinic.lat);

                var clinicContainer = new ClinicContainer()
                {
                    name = clinic.name,
                    address = clinic.address,
                    services = svc,
                    coordinates = coordi
                };
                lst.Add(clinicContainer);
            }
            return View(lst);
        }
    }
}
