using Microsoft.AspNetCore.Mvc;
using HealthyAus.Models;
using System.Diagnostics;

namespace HealthyAus.Controllers
{
    public class STIController : Controller
    {
        private readonly STDContext _stdContext;
        private readonly ILogger<STIController> _logger;

        public STIController(ILogger<STIController> logger, STDContext sTDContext)
        {
            _logger = logger;
            _stdContext = sTDContext;
        }

        public IActionResult Index()
        {
            //query the database to get all stis and thier effects/sysmtoms/protective_measurements
            var effects = from sti_effect in _stdContext.STI_EFFECT
                          join sti in _stdContext.STI
                          on sti_effect.sti_id equals sti.sti_id
                          join effect in _stdContext.Effect
                          on sti_effect.effect_id equals effect.effect_id
                          select new { stiName = sti.sti_name, stiEffect = effect.effect };
            var symptoms = from sti_symptoms in _stdContext.STI_Symptom
                           join sti in _stdContext.STI
                           on sti_symptoms.sti_id equals sti.sti_id
                           join symptom in _stdContext.Symptom
                           on sti_symptoms.symptom_id equals symptom.symptom_id
                           select new { stiName = sti.sti_name, stiSymptoms = symptom.symptom };
            var protects = from sti_protects in _stdContext.STI_Protect
                           join sti in _stdContext.STI
                           on sti_protects.sti_id equals sti.sti_id
                           join protect in _stdContext.protective_Measure
                           on sti_protects.protect_id equals protect.protect_id
                           select new { stiName = sti.sti_name, stiProtect = protect.protect_meas };
            var stis = from sti in _stdContext.STI
                       select sti;

            //put data into stiContainer Model to be bundled into viewmodel that will be passed to view
            STIViewModel sTIViewModel = new STIViewModel();
            List<string> stinames = new List<string>();
            foreach (var st in stis)
            {
                stinames.Add(st.sti_name);
            }
            foreach (var st in stinames)
            {
                string stiName = st;
                List<string> stiEffects = new List<string>();
                List<String> stiProtect = new List<string>();
                List<string> stiSymptoms = new List<string>();
                foreach (var protect in protects)
                {
                    if (protect.stiName.Equals(stiName))
                    {
                        stiProtect.Add(protect.stiProtect);
                    }
                }
                foreach (var symptom in symptoms)
                {
                    if (symptom.stiName.Equals(stiName))
                    {
                        stiSymptoms.Add(symptom.stiSymptoms);
                    }
                }
                foreach (var effect in effects)
                {
                    if (effect.stiName.Equals(stiName))
                    {
                        stiEffects.Add(effect.stiEffect);
                    }
                }
                //Create stiContainer
                STIContainer stiContainer = new STIContainer()
                {
                    Name = stiName,
                    Effects = stiEffects,
                    Symptoms = stiSymptoms,
                    Protective_measures = stiProtect
                };
                //put stiContainer into viewmodel 
                if (stiContainer != null)
                {
                    sTIViewModel.STIContainers.Add(stiContainer);
                }

            }


            return View(sTIViewModel.STIContainers);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
