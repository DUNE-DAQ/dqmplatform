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
    public class DataController : Controller
    {
        private readonly MonitoringDbContext _context;

        public DataController(MonitoringDbContext context)
        {
            _context = context;

            var toDeletes = _context.Data.Where(d => d.Name.Contains("hist_")).ToList();

            foreach (var toDelete in toDeletes)
            {
                _context.Data.Remove(toDelete);
                var dataPaths = _context.DataPaths.Include(ddd => ddd.Data).Where(ddd => ddd.Data == toDelete).ToList();
                foreach (var dataPath in dataPaths)
                {
                    _context.DataPaths.Remove(dataPath);
                }
            }
            _context.SaveChanges();

        }

        // GET: Data
        public async Task<IActionResult> Index()
        {
            return View(await _context.Data.Include(d=> d.DataSource).ToListAsync());
        }

        // GET: Data/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _context.Data.Include(d => d.DataSource).FirstOrDefaultAsync(m => m.Id == id);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        // GET: Data/Create
        public IActionResult Create()
        {
            ViewBag.DataSources = _context.DataSources.ToList();

            return View();
        }

        // POST: Data/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RententionTime,Name")] Models.Data data, Guid dataSourceId)
        {

            data.DataSource = _context.DataSources.Where(d => d.Id == dataSourceId).FirstOrDefault();


            if (ModelState.IsValid)
            {
                data.Id = Guid.NewGuid();
                _context.Add(data);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(data);
        }

        // GET: Data/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _context.Data.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }

            ViewBag.DataSources = _context.DataSources.ToList();


            return View(data);
        }

        // POST: Data/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,RententionTime,Name")] Models.Data data, Guid dataSourceId)
        {
            if (id != data.Id)
            {
                return NotFound();
            }

            data.DataSource = _context.DataSources.Where(d => d.Id == dataSourceId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(data);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataExists(data.Id))
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
            return View(data);
        }

        // GET: Data/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _context.Data
                .FirstOrDefaultAsync(m => m.Id == id);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        // POST: Data/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var data = await _context.Data.FindAsync(id);
            _context.Data.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DataExists(Guid id)
        {
            return _context.Data.Any(e => e.Id == id);
        }
    }
}
