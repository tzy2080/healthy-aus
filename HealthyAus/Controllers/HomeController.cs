using HealthyAus.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
//using System.Linq;


namespace HealthyAus.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly STDContext _stdContext;

        public HomeController(ILogger<HomeController> logger, STDContext sTDContext)
        {
            _logger = logger;
            _stdContext = sTDContext;
        }

        public async Task<IActionResult> Index()
        {
            /** List<STI> list = new List<STI>();
            list = await _stdContext.STI.ToListAsync();
            int x = 0;
            while (x < list.Count)
            {
                STI st = list[x];
                x++;
            }

            List<String> lst = new List<string>();

            var stis = from sti_effect in _stdContext.STI_EFFECT
                        join sti in _stdContext.STI on sti_effect.sti_id equals sti.sti_id into sti_effect_grp
                        from sti in sti_effect_grp
                        join effect in _stdContext.Effect on sti_effect.sti_id equals effect.effect_id
                        select new { STIName = sti.sti_name, EffectName = effect.effect };
            
          
          
            foreach (var st in stis)
            {
                string dis = st.STIName;
                string thatd = st.EffectName;
                string biubiu = dis + thatd;
                lst.Add(biubiu);
            }
            
            var stis = from sti_effect in _stdContext.STI_EFFECT
                       join sti in _stdContext.STI
                       on sti_effect.sti_id equals sti.sti_id 
                       join effect in _stdContext.Effect 
                       on sti_effect.effect_id equals effect.effect_id
                       select new {stiName = sti.sti_name, stiEffect = effect.effect};
            foreach (var st in stis)
            {
                string dis = st.stiEffect;
                string thatd = st.stiName;
                string biubiu = dis + thatd;
                lst.Add(biubiu);

            }

            ViewBag.Message = lst;**/
            return View();  
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/Home/Error/{code:int}")]
        public IActionResult Error(int code)
        {
            return View(new ErrorViewModel { RequestId = "Test", ErrorMessage = $"Error Code {code}" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


    //creating dbContext and DbSet
    public class STDContext : DbContext
    {
        public STDContext(DbContextOptions<STDContext> options) : base(options) { }
        public DbSet<STI> STI { get; set; }
        public DbSet<Stats> Stats { get; set; }
        public DbSet<STI_EFFECT> STI_EFFECT { get; set; }
        public DbSet<Effect> Effect { get; set; }
        public DbSet<Symptom> Symptom { get; set; }
        public DbSet<STI_Symptom> STI_Symptom { get; set; }
        public DbSet<Protective_Measure> protective_Measure { get; set; }
        public DbSet<STI_Protect> STI_Protect { get; set; }
        public DbSet<STI_CASE_PER_100K> STI_CASE_PER_100K { get; set; }
        public DbSet<AGE_GRP> AGE_GRP { get; set; }
        public DbSet<LOCATION> LOCATION { get; set; }
        public DbSet<Sex> Sex { get; set; }
        public DbSet<YEAR> YEAR { get; set; }
        public DbSet<Remoteness> Remoteness { get; set; }
        public DbSet<CLINIC> CLINIC { get; set; }

        public DbSet<CONTACTTRACING> CONTACTTRACING { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<STI_EFFECT>().HasKey(x => new {x.sti_id,x.effect_id});
    
        }

    }
}
