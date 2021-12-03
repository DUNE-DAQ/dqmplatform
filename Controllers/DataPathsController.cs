using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DuneDaqMonitoringPlatform.Data;
using DuneDaqMonitoringPlatform.Models;
using Microsoft.AspNetCore.Authorization;

namespace DuneDaqMonitoringPlatform.Controllers
{
    [Authorize(Roles = "Administrator, User")]
    public class DataPathsController : Controller
    {
        private readonly MonitoringDbContext _context;

        public DataPathsController(MonitoringDbContext context)
        {
            _context = context;
        }

        // GET: DataPaths
        public async Task<IActionResult> Index()
        {
            return View(await _context.DataPaths.ToListAsync());
        }

        // GET: DataPaths/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataPath = await _context.DataPaths
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataPath == null)
            {
                return NotFound();
            }

            return View(dataPath);
        }

        // GET: DataPaths/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: DataPaths/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WriteTime,Path")] DataPath dataPath)
        {
            if (ModelState.IsValid)
            {
                dataPath.Id = Guid.NewGuid();
                _context.Add(dataPath);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dataPath);
        }

        // GET: DataPaths/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataPath = await _context.DataPaths.FindAsync(id);
            if (dataPath == null)
            {
                return NotFound();
            }
            return View(dataPath);
        }

        // POST: DataPaths/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,WriteTime,Path")] DataPath dataPath)
        {
            if (id != dataPath.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dataPath);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataPathExists(dataPath.Id))
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
            return View(dataPath);
        }

        // GET: DataPaths/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataPath = await _context.DataPaths
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataPath == null)
            {
                return NotFound();
            }

            return View(dataPath);
        }

        // POST: DataPaths/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var dataPath = await _context.DataPaths.FindAsync(id);
            _context.DataPaths.Remove(dataPath);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DataPathExists(Guid id)
        {
            return _context.DataPaths.Any(e => e.Id == id);
        }
    }
}
