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
    public class DataSourcesController : Controller
    {
        private readonly MonitoringDbContext _context;

        
        public DataSourcesController(MonitoringDbContext context)
        {
            _context = context;
        }

        // GET: DataSources
        public async Task<IActionResult> Index()
        {
            return View(await _context.DataSources.ToListAsync());
        }

        // GET: DataSources/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataSource = await _context.DataSources
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataSource == null)
            {
                return NotFound();
            }

            return View(dataSource);
        }

        // GET: DataSources/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: DataSources/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Source,Description")] DataSource dataSource)
        {
            if (ModelState.IsValid)
            {
                dataSource.Id = Guid.NewGuid();
                _context.Add(dataSource);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dataSource);
        }

        // GET: DataSources/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataSource = await _context.DataSources.FindAsync(id);
            if (dataSource == null)
            {
                return NotFound();
            }
            return View(dataSource);
        }

        // POST: DataSources/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Source,Description")] DataSource dataSource)
        {
            if (id != dataSource.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dataSource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataSourceExists(dataSource.Id))
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
            return View(dataSource);
        }

        // GET: DataSources/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataSource = await _context.DataSources
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataSource == null)
            {
                return NotFound();
            }

            return View(dataSource);
        }

        // POST: DataSources/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var dataSource = await _context.DataSources.FindAsync(id);
            _context.DataSources.Remove(dataSource);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DataSourceExists(Guid id)
        {
            return _context.DataSources.Any(e => e.Id == id);
        }
    }
}
