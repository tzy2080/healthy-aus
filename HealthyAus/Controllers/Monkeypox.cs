using Microsoft.AspNetCore.Mvc;

namespace HealthyAus.Controllers
{
    public class Monkeypox : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
