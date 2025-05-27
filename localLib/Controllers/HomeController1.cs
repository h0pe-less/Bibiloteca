using Microsoft.AspNetCore.Mvc;

namespace localLib.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
