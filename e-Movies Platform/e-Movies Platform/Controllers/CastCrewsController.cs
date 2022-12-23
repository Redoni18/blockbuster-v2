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
using Microsoft.EntityFrameworkCore.Metadata;

namespace e_Movies_Platform.Controllers
{
    public class CastCrewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CastCrewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CastCrews
        public async Task<IActionResult> Index()
        {
                //return View(await _context.CastCrew.ToListAsync());
                return View(await _context.CastCrew.Include(a => a.Role).ToListAsync());
        }

        // GET: CastCrews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CastCrew == null)
            {
                return NotFound();
            }

            var castCrew = await _context.CastCrew
                .FirstOrDefaultAsync(m => m.Id == id);
            if (castCrew == null)
            {
                return NotFound();
            }

            return View(castCrew);
        }

        // GET: CastCrews/Create
        public IActionResult Create()
        {
            //List<CastCrewRole> roles = _context.CastCrewRole.Select(x => new CastCrewRole { Value = x.Id.ToString(), Text = x.Role}).ToList();
            List<CastCrewRole> roles = this._context.CastCrewRole.ToList();
            CastCrewViewModel model = new CastCrewViewModel();

            model.Roles = roles;
            return View(model);
        }

        // POST: CastCrews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CastCrewViewModel model)
        {
            var role = await this._context.CastCrewRole.FindAsync(model.RoleId);
            var castcrew = new CastCrew();
            if(role != null)
            {
                castcrew.Role = role;
                castcrew.FullName = model.FullName;

                this._context.CastCrew.Add(castcrew);
                this._context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: CastCrews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CastCrew == null)
            {
                return NotFound();
            }
            

            var castCrew = await _context.CastCrew.FindAsync(id);

            if (castCrew == null)
            {
                return NotFound();
            }

            return View(castCrew);
        }

        // POST: CastCrews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,FullName")] CastCrew castCrew)
        {
            if (id != castCrew.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(castCrew);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CastCrewExists(castCrew.Id))
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
            return View(castCrew);
        }

        // GET: CastCrews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CastCrew == null)
            {
                return NotFound();
            }

            var castCrew = await _context.CastCrew
                .FirstOrDefaultAsync(m => m.Id == id);
            if (castCrew == null)
            {
                return NotFound();
            }

            return View(castCrew);
        }

        // POST: CastCrews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.CastCrew == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CastCrew'  is null.");
            }
            var castCrew = await _context.CastCrew.FindAsync(id);
            if (castCrew != null)
            {
                _context.CastCrew.Remove(castCrew);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CastCrewExists(int? id)
        {
          return _context.CastCrew.Any(e => e.Id == id);
        }
    }
}
