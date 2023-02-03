using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using e_Movies_Platform.Data;
using e_Movies_Platform.Models;
using Microsoft.AspNetCore.Authorization;

namespace e_Movies_Platform.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class CastCrewRolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CastCrewRolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CastCrewRoles
        public async Task<IActionResult> Index(int pg=1, string searchString = "", string sortOrder = "")  
        {
            ViewData["RoleSortParm"] = sortOrder == "Role" ? "role_desc" : "Role";
            ViewData["CurrentFilter"] = searchString;
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;

            List<CastCrewRole> castCrewRoles =await _context.CastCrewRole.ToListAsync();
            if (!String.IsNullOrEmpty(searchString))
            {
                castCrewRoles = await _context.CastCrewRole.Where(c => c.Role.ToLower().Contains(searchString.ToLower())).ToListAsync();
            }

            if (!String.IsNullOrEmpty(sortOrder)) 
            { 
                switch(sortOrder)
                {
                    case "Role":
                        castCrewRoles = await _context.CastCrewRole.OrderBy(c => c.Role).ToListAsync();
                        break;
                    case "role_desc":
                        castCrewRoles = await _context.CastCrewRole.OrderByDescending(c => c.Role).ToListAsync();
                        break;
                }
            }

            int recsCount = castCrewRoles.Count();

            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = castCrewRoles.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;

            return View(data);

            //return View(await _context.CastCrewRole.ToListAsync());
        }

        // GET: CastCrewRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CastCrewRole == null)
            {
                return NotFound();
            }

            var castCrewRole = await _context.CastCrewRole
                .FirstOrDefaultAsync(m => m.id == id);
            if (castCrewRole == null)
            {
                return NotFound();
            }

            return View(castCrewRole);
        }

        // GET: CastCrewRoles/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: CastCrewRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("id,Role")] CastCrewRole castCrewRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(castCrewRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(castCrewRole);
        }

        // GET: CastCrewRoles/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CastCrewRole == null)
            {
                return NotFound();
            }

            var castCrewRole = await _context.CastCrewRole.FindAsync(id);
            if (castCrewRole == null)
            {
                return NotFound();
            }
            return View(castCrewRole);
        }

        // POST: CastCrewRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("id,Role")] CastCrewRole castCrewRole)
        {
            if (id != castCrewRole.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(castCrewRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CastCrewRoleExists(castCrewRole.id))
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
            return View(castCrewRole);
        }

        // GET: CastCrewRoles/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CastCrewRole == null)
            {
                return NotFound();
            }

            var castCrewRole = await _context.CastCrewRole
                .FirstOrDefaultAsync(m => m.id == id);
            if (castCrewRole == null)
            {
                return NotFound();
            }

            return View(castCrewRole);
        }

        // POST: CastCrewRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CastCrewRole == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CastCrewRole'  is null.");
            }
            var castCrewRole = await _context.CastCrewRole.FindAsync(id);
            if (castCrewRole != null)
            {
                _context.CastCrewRole.Remove(castCrewRole);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CastCrewRoleExists(int id)
        {
          return _context.CastCrewRole.Any(e => e.id == id);
        }
    }
}
