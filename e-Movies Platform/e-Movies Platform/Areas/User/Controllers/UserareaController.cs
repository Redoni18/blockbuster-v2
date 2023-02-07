using e_Movies_Platform.Data;
using e_Movies_Platform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace e_Movies_Platform.Areas.User.Controllers
{
    [Area("User")]
    public class UserareaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserareaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> IndexAsync(int pg = 1, string searchString = "")
        {
            ViewData["CurrentFilter"] = searchString;
            const int pageSize = 9;
            if (pg < 1)
                pg = 1;

            List<Movie> movies = await _context.Movie.Include(m => m.Genre).Include(m => m.Cast).ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = await _context.Movie.Where(m => m.Title.ToLower().Contains(searchString.ToLower())).ToListAsync();
            }

            int recsCount = movies.Count();

            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = movies.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;

            return View(data);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> MovieDetails(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            List<CastCrewRole> roles = this._context.CastCrewRole.ToList();

            var movie = await _context.Movie.Include(m => m.Genre).Include(m => m.Cast)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
    }
}
