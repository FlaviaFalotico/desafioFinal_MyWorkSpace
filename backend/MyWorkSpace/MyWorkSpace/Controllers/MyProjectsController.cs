using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWorkSpace.Areas.Identity.Data;
using MyWorkSpace.Models;

namespace MyWorkSpace.Controllers
{
    public class MyProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MyProjects
        public async Task<IActionResult> Index()
        {
            string email = HttpContext.User.Identity.Name;
            string UserId = _context.Users.FirstOrDefault(u => u.Email == email).Id;
            var applicationDbContext = _context.MyProjects.Where(m => m.UserId == UserId);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MyProjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MyProjects == null)
            {
                return NotFound();
            }

            var myProject = await _context.MyProjects
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myProject == null)
            {
                return NotFound();
            }

            return View(myProject);
        }

        // GET: MyProjects/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: MyProjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MyProject myProject)
        {
            if (myProject != null)
            {
                string email = HttpContext.User.Identity.Name;
                myProject.UserId = _context.Users.FirstOrDefault(u => u.Email == email).Id;
                _context.Add(myProject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", myProject.UserId);
            return View(myProject);
        }

        // GET: MyProjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string email = HttpContext.User.Identity.Name;
            string UserId = _context.Users.FirstOrDefault(u => u.Email == email).Id;
            if (id == null || _context.MyProjects == null)
            {
                return NotFound();
            }

            var myProject = _context.MyProjects.Where(p => p.UserId == UserId).FirstOrDefault(p => p.Id == id);
            if (myProject == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", myProject.UserId);
            return View(myProject);
        }

        // POST: MyProjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CreatedDate,UserId")] MyProject myProject)
        {
            if (id != myProject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(myProject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MyProjectExists(myProject.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", myProject.UserId);
            return View(myProject);
        }

        // GET: MyProjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MyProjects == null)
            {
                return NotFound();
            }

            var myProject = await _context.MyProjects
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myProject == null)
            {
                return NotFound();
            }

            return View(myProject);
        }

        // POST: MyProjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MyProjects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MyProjects'  is null.");
            }
            var myProject = await _context.MyProjects.FindAsync(id);
            if (myProject != null)
            {
                _context.MyProjects.Remove(myProject);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MyProjectExists(int id)
        {
          return (_context.MyProjects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
