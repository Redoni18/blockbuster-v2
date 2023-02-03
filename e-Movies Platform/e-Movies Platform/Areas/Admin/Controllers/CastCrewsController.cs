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
using Microsoft.AspNetCore.Authorization;
using Razorpay.Api;

namespace e_Movies_Platform.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class CastCrewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CastCrewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CastCrews
        public async Task<IActionResult> Index(int pg=1, string searchString = "", string sortOrder = "")
        {
            //return View(await _context.CastCrew.Include(a => a.Role).ToListAsync());
            ViewData["StringSortParm"] = sortOrder == "FullName" ? "fullName_desc" : "FullName";
            ViewData["CurrentFilter"] = searchString;
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;

            List<CastCrew> castCrews =await _context.CastCrew.Include(a => a.Role).ToListAsync();
            if (!String.IsNullOrEmpty(searchString))
            {
                castCrews = await _context.CastCrew.Where(m => m.FullName.ToLower().Contains(searchString.ToLower())).ToListAsync();
            }

            if (!String.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "Fullname":
                        castCrews = await _context.CastCrew.OrderBy(m => m.FullName).ToListAsync();
                        break;
                    case "fullName_desc":
                        castCrews = await _context.CastCrew.OrderByDescending(m => m.FullName).ToListAsync();
                        break;
                }
            }

            int recsCount = castCrews.Count();

            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = castCrews.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;

            return View(data);

            //return View(await _context.CastCrew.ToListAsync());
            
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CastCrew == null)
            {
                return NotFound();
            }


            var castCrew = await _context.CastCrew.Include(c => c.Role)
                .FirstOrDefaultAsync(m => m.Id == id);

            List<CastCrewRole> roles = this._context.CastCrewRole.ToList();
            CastCrewViewModel model = new CastCrewViewModel();

            model.Id = (int)castCrew.Id;
            model.FullName = castCrew.FullName;
            model.RoleId = castCrew.Role.id;
            model.Role = castCrew.Role;
            model.Roles = roles;
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: CastCrews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,FullName,RoleId")] CastCrewViewModel model)
        {
            var castCrew = await _context.CastCrew.Include(c => c.Role)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (id != castCrew.Id)
            {
                return NotFound();
            }
            var castCrewRole = await this._context.CastCrewRole.FindAsync(model.RoleId);

            castCrew.Role = castCrewRole;

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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
