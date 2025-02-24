using Microsoft.AspNetCore.Mvc;

namespace _11_ActorManagementApp.Controllers
{
    public class MovieController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
