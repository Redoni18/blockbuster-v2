﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using e_Movies_Platform.Data;
using e_Movies_Platform.Models;

namespace e_Movies_Platform.Controllers
{
    public class CastCrewRolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CastCrewRolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CastCrewRoles
        public async Task<IActionResult> Index()
        {
              return View(await _context.CastCrewRole.ToListAsync());
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: CastCrewRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
