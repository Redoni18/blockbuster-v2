using e_Movies_Platform.Data;
using e_Movies_Platform.Data.Migrations;
using e_Movies_Platform.Models;
using e_Movies_Platform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Data;

namespace e_Movies_Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        //private readonly UserManager<ApplicationUser> _userManager;

        //public UsersController(UserManager<ApplicationUser> userManager)
        //{
        //    _userManager = userManager;
        //}

        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int pg = 1, string searchString = "", string sortOrder = "")
        {
            ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["CurrentFilter"] = searchString;
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;

            var users = await _context.Users.ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                users = await _context.Users.Where(m => m.Name.ToLower().Contains(searchString.ToLower())).ToListAsync();
            }

            if (!String.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "Name":
                        users = await _context.Users.OrderBy(m => m.Name).ToListAsync();
                        break;
                    case "name_desc":
                        users = await _context.Users.OrderByDescending(m => m.Name).ToListAsync();
                        break;                 
                }
            }


            int recsCount = users.Count();

            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;

            var data = users.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;

            var usersViewModel = new List<UsersViewModel>();
            foreach (ApplicationUser user in data)
            {
                var thisViewModel = new UsersViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.Name = user.Name;
                thisViewModel.LastName = user.LastName;
                thisViewModel.Birthday = (DateTime)user.Birthday;
                thisViewModel.EmailConfirmed = user.EmailConfirmed;
                usersViewModel.Add(thisViewModel);
            }

            return View(usersViewModel);
        }

        // GET:
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Users'  is null.");
            }
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user != null)
            {
               _context.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
