using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWorkSpace.Areas.Identity.Data;
using MyWorkSpace.Models;

namespace MyWorkSpace.Controllers
{
    [Authorize]
    public class MyTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MyTasks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MyTasks.Include(m => m.MyProject);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MyTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MyTasks == null)
            {
                return NotFound();
            }

            var myTask = await _context.MyTasks
                .Include(m => m.MyProject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myTask == null)
            {
                return NotFound();
            }

            return View(myTask);
        }

        // GET: MyTasks/Create
        public IActionResult Create()
        {
            ViewData["MyProjectId"] = new SelectList(_context.MyProjects, "Id", "Description");
            return View();
        }

        // POST: MyTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CreatedDate,MyProjectId")] MyTask myTask)
        {          
            
            _context.Add(myTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
            ViewData["MyProjectId"] = new SelectList(_context.MyProjects, "Id", "Description", myTask.MyProjectId);
            return View(myTask);
        }

        // GET: MyTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MyTasks == null)
            {
                return NotFound();
            }

            var myTask = await _context.MyTasks.FindAsync(id);
            if (myTask == null)
            {
                return NotFound();
            }
            ViewData["MyProjectId"] = new SelectList(_context.MyProjects, "Id", "Description", myTask.MyProjectId);
            return View(myTask);
        }

        // POST: MyTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CreatedDate,MyProjectId")] MyTask myTask)
        {
            if (id != myTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(myTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MyTaskExists(myTask.Id))
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
            ViewData["MyProjectId"] = new SelectList(_context.MyProjects, "Id", "Description", myTask.MyProjectId);
            return View(myTask);
        }

        // GET: MyTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MyTasks == null)
            {
                return NotFound();
            }

            var myTask = await _context.MyTasks
                .Include(m => m.MyProject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myTask == null)
            {
                return NotFound();
            }

            return View(myTask);
        }

        // POST: MyTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MyTasks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MyTasks'  is null.");
            }
            var myTask = await _context.MyTasks.FindAsync(id);
            if (myTask != null)
            {
                _context.MyTasks.Remove(myTask);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MyTaskExists(int id)
        {
          return (_context.MyTasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
