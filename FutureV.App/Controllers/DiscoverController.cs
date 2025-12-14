using Microsoft.AspNetCore.Mvc;

namespace FutureV.Controllers
{
    public class DiscoverController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
