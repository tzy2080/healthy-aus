using Microsoft.AspNetCore.Mvc;
using HealthyAus.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace HealthyAus.Controllers
{
    public class StatsController : Controller
    {
        private readonly STDContext _stdContext;
        private readonly ILogger<STIController> _logger;

        public StatsController(ILogger<STIController> logger, STDContext sTDContext)
        {
            _logger = logger;
            _stdContext = sTDContext;
        }
        public IActionResult Index()
        {
            //pass the R shiny app address to view
            ViewBag.IframeLink = "https://healthyaus.shinyapps.io/hiv_cd4_count/";
            //query database to get sti_case info
            var stiCase = from sti_case in _stdContext.STI_CASE_PER_100K
                          join sti in _stdContext.STI
                          on sti_case.sti_id equals sti.sti_id
                          join year in _stdContext.YEAR
                          on sti_case.year_id equals year.year_id
                          join age in _stdContext.AGE_GRP
                          on sti_case.age_id equals age.age_id
                          join locaiton in _stdContext.LOCATION
                          on sti_case.loc_id equals locaiton.loc_id
                          join remoteness in _stdContext.Remoteness
                          on sti_case.remote_id equals remoteness.remote_id
                          join sex in _stdContext.Sex
                          on sti_case.sex_id equals sex.sex_id
                          select new
                          {
                              stiName = sti.sti_name,
                              stiYear = year.year,
                              stiAge = age.age_grp,
                              stiLoc = locaiton.loc,
                              stiSex = sex.sex,
                              stiRemote = remoteness.remoteness,
                              stiRate = sti_case.rate
                          };

            //put sti case info into sticaseContainer to be put into sticaseViewModel which will be passed to View
            STICaseViewModel sTICaseViewModel = new STICaseViewModel();
            foreach (var cas in stiCase)
            {
                STICaseContainer sTICaseContainer = new STICaseContainer()
                {
                    sti = cas.stiName,
                    year = cas.stiYear,
                    age = cas.stiAge,
                    loc = cas.stiLoc,
                    sex = cas.stiSex,
                    remote = cas.stiRemote,
                    rate = cas.stiRate
                };
                sTICaseViewModel.STICaseContainers.Add(sTICaseContainer);
            }


            return View(sTICaseViewModel.STICaseContainers);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


   /** public class STDContext : DbContext
    {
        public STDContext(DbContextOptions<STDContext> options) : base(options) { }
        public DbSet<STI> STIs { get; set; }
        public DbSet<Stats> Stats { get; set; }

    }**/
}
