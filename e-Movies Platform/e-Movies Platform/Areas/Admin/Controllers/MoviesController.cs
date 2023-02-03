using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using e_Movies_Platform.Data;
using e_Movies_Platform.Models;
using e_Movies_Platform.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using e_Movies_Platform.Data.Migrations;
using EllipticCurve;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace e_Movies_Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        //[Authorize]
        public async Task<IActionResult> Index(int pg = 1, string searchString = "", string sortOrder = "")
        {


            //ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Year" ? "year_desc" : "Year";
            ViewData["CurrentFilter"] = searchString;
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;

            List<Movie> movies = await _context.Movie.Include(m => m.Genre).Include(m => m.Cast).ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = await _context.Movie.Where(m => m.Title.ToLower().Contains(searchString.ToLower())).ToListAsync();
            }

            if(!String.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    //case "name_desc":
                    //    movies = await _context.Movie.OrderByDescending(m => m.Title).ToListAsync();
                    //    break;
                    case "Year":
                        movies = await _context.Movie.OrderBy(m => m.Year).ToListAsync();
                        break;
                    case "year_desc":
                        movies = await _context.Movie.OrderByDescending(m => m.Year).ToListAsync();
                        break;
                    //default:
                    //    movies = await _context.Movie.OrderBy(m => m.Title).ToListAsync();
                    //    break;
                }
            }


            int recsCount = movies.Count();

            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = movies.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;

            return View(data);

        }

        // GET: Movies/Details/5
        //[Authorize]
        public async Task<IActionResult> Details(int? id)
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

        // GET: Movies/Create
        //[Authorize]
        //[Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            List<Genre> genres = this._context.Genre.ToList();
            List<CastCrew> cast = this._context.CastCrew.ToList();
            List<CastCrewRole> roles = this._context.CastCrewRole.ToList();
            MovieViewModel model = new MovieViewModel(); 

            model.Genres = genres;
            model.Cast = cast;
            model.Roles = roles;
            return View(model);
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(MovieViewModel model)
        {
            var genre = await this._context.Genre.FindAsync(model.GenreId);
            var cast = new List<CastCrew>();
            foreach(int castId in model.CastId)
            {
                var castMember = await this._context.CastCrew.FindAsync(castId);
                cast.Add(castMember);
            }
            var director = await this._context.CastCrew.FindAsync(model.DirectorId);
            cast.Add(director);
            var movie = new Movie();
            movie.Title = model.Title;
            movie.Description = model.Description;
            movie.IsPG = model.IsPG;
            movie.CoverImage = model.CoverImage;
            movie.MovieLink = model.MovieLink;
            movie.Genre = genre;
            movie.Year = model.Year;
            movie.Cast = cast;

            this._context.Movie.Add(movie);
            this._context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Movies/Edit/5
        //[Authorize]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }
            List<Genre> genres = this._context.Genre.ToList();
            List<CastCrew> cast = this._context.CastCrew.ToList();
            List<CastCrewRole> roles = this._context.CastCrewRole.ToList();

            
            var movie = await _context.Movie.Include(m => m.Genre).Include(m => m.Cast)   
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }
             


            MovieViewModel model = new MovieViewModel();
            model.Id = movie.Id;
            model.Title = movie.Title;
            model.Description = movie.Description;
            model.CoverImage = movie.CoverImage;
            model.MovieLink = movie.MovieLink;
            model.Year = movie.Year;
            model.IsPG = (bool)movie.IsPG;
            model.GenreId = movie.Genre.Id;
            model.Genres = genres;
            model.Genre = movie.Genre;
            model.Director = (CastCrew?)movie.Cast.Where(c => c.Role.Role == "Director").FirstOrDefault();
            model.SelectedCast = movie.Cast.Where(c => c.Role.Role == "Actor").ToList();
            model.NotSelectedCast =cast.Except(model.SelectedCast).ToList();
            model.Cast = cast;
            

            if (model == null)
            {
                return NotFound();
            }
          return View(model);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CoverImage,MovieLink,IsPG,Year,Cast,Genre,DirectorId,CastId,GenreId")] MovieViewModel model)
        {

            List<Genre> allGenres = this._context.Genre.ToList();
            List<CastCrew> allCast = this._context.CastCrew.ToList();
            List<CastCrewRole> roles = this._context.CastCrewRole.ToList();

            var movie = await _context.Movie.Include(m => m.Genre).Include(m => m.Cast)
                .FirstOrDefaultAsync(m => m.Id == id);
    

            model.Genres = allGenres;
            model.Cast = allCast;
            model.Roles = roles;

            var cast = new List<CastCrew>();
            foreach (int castId in model.CastId)
            {
                var castMember = await this._context.CastCrew.FindAsync(castId);
                cast.Add(castMember);
            }
            var director = await this._context.CastCrew.FindAsync(model.DirectorId);
            cast.Add(director);
            model.Director = director;

            List<CastCrew> updatedCast = cast.Union(movie.Cast.Intersect(cast)).ToList();
            model.SelectedCast = updatedCast;
            model.NotSelectedCast = updatedCast.Except(updatedCast).ToList();

            movie.Title = model.Title;
            movie.Description = model.Description;
            movie.IsPG = model.IsPG;
            movie.CoverImage = model.CoverImage;
            movie.MovieLink = model.MovieLink;
            movie.Year = model.Year;
            movie.Cast = updatedCast;

            var genre = await this._context.Genre.FindAsync(model.Genre.Id);
            movie.Genre = genre;

            if (id != movie.Id)
            {
                return NotFound();
            }

            //IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

            //var errors = ModelState.Keys
            //       .Where(k => ModelState[k].Errors.Count > 0)
            //       .Select(k => new
            //       {
            //           propertyName = k,
            //           errorMessage = ModelState[k].Errors[0].ErrorMessage
            //       }).ToList();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        //[Authorize]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Authorize]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
