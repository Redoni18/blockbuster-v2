using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using e_Movies_Platform.Data;
using e_Movies_Platform.ViewModels;
using e_Movies_Platform.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace e_Movies_Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class MovieRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/MovieRequests
        public async Task<IActionResult> Index(int pg = 1, string searchString = "", string sortOrder = "")
        {


            //ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["CurrentFilter"] = searchString;
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;


            List<WishList> requests = await _context.WishList.Include(m => m.User).ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                requests = await _context.WishList.Where(m => m.Name.ToLower().Contains(searchString.ToLower())).ToListAsync();
            }

            if (!String.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "Name":
                        requests = await _context.WishList.OrderBy(m => m.Name).ToListAsync();
                        break;
                    case "name_desc":
                        requests = await _context.WishList.OrderByDescending(m => m.Name).ToListAsync();
                        break;
                }
            }


            int recsCount = requests.Count();

            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = requests.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;

            return View(data);

        }
            // GET: Admin/MovieRequests/Details/5
        //    public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.MovieRequests == null)
        //    {
        //        return NotFound();
        //    }

        //    var movieRequests = await _context.MovieRequests
        //        .Include(m => m.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (movieRequests == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(movieRequests);
        //}

        // POST: Admin/MovieRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<IActionResult> Edit(int id)
        {
            var movieRequests = await this._context.WishList.Include(m => m.User).FirstOrDefaultAsync(m => m.Id == id);

            if (id != movieRequests.Id)
            {
                return NotFound();
            }

            movieRequests.Isapproved = true;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieRequests);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieRequestsExists((int)movieRequests.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movieRequests);
        }

        //// GET: Admin/MovieRequests/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.MovieRequests == null)
        //    {
        //        return NotFound();
        //    }

        //    var movieRequests = await _context.MovieRequests
        //        .Include(m => m.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (movieRequests == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(movieRequests);
        //}

        //// POST: Admin/MovieRequests/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.MovieRequests == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.MovieRequests'  is null.");
        //    }
        //    var movieRequests = await _context.MovieRequests.FindAsync(id);
        //    if (movieRequests != null)
        //    {
        //        _context.MovieRequests.Remove(movieRequests);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool MovieRequestsExists(int id)
        {
          return _context.WishList.Any(e => e.Id == id);
        }
    }
}
