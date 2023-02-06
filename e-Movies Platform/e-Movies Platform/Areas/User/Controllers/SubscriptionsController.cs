using e_Movies_Platform.Data;
using e_Movies_Platform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace e_Movies_Platform.Areas.User.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public SubscriptionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Area("User")]
        public IActionResult Index()
        {
            List<Subscription> subscriptions = _context.Subscription.ToList();

            return View(subscriptions);
        }
        [Area("User")]
        public IActionResult Pay()
        {
            return PartialView("_PaymentView");
        }
        [Area("User")]
        //[HttpPost]
        public async Task<IActionResult> Subscribe()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            user.isSubscribed = true;
            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index", "Userarea");
        }
    }
}
