using e_Movies_Platform.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace e_Movies_Platform.Controllers
{
    public class HomeeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}