using Microsoft.AspNetCore.Mvc;

namespace e_Movies_Platform.Areas.User.Controllers
{
    public class UserareaController : Controller
    {
        [Area("User")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
