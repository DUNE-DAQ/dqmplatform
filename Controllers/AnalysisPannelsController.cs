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
    public class AnalysisPannelsController : Controller
    {
        private readonly MonitoringDbContext _context;

        public AnalysisPannelsController(MonitoringDbContext context)
        {
            _context = context;
        }

        // GET: AnalysisPannels
        public async Task<IActionResult> Index()
        {
            return View(await _context.AnalysisPannel.Include(d => d.DataDisplay).Include(d => d.Pannel).ToListAsync());
        }

        // GET: AnalysisPannels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysisPannel = await _context.AnalysisPannel.Include(d => d.DataDisplay).Include(d => d.Pannel).FirstOrDefaultAsync(m => m.Id == id);
            if (analysisPannel == null)
            {
                return NotFound();
            }

            return View(analysisPannel);
        }

        // GET: AnalysisPannels/Create
        public IActionResult Create()
        {
            ViewBag.Pannels = _context.Pannel.ToList();
            ViewBag.DataDisplays = _context.DataDisplay.ToList();

            return View();
        }

        // POST: AnalysisPannels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] AnalysisPannel analysisPannel, Guid pannelId, Guid dataDisplayId)
        {

            analysisPannel.Pannel = _context.Pannel.Where(d => d.Id == pannelId).FirstOrDefault();
            analysisPannel.DataDisplay = _context.DataDisplay.Where(d => d.Id == dataDisplayId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                analysisPannel.Id = Guid.NewGuid();
                _context.Add(analysisPannel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(analysisPannel);
        }

        // GET: AnalysisPannels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Pannels = _context.Pannel.ToList();
            ViewBag.DataDisplays = _context.DataDisplay.ToList();

            var analysisPannel = await _context.AnalysisPannel.FindAsync(id);
            if (analysisPannel == null)
            {
                return NotFound();
            }
            return View(analysisPannel);
        }

        // POST: AnalysisPannels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id")] AnalysisPannel analysisPannel, Guid pannelId, Guid dataDisplayId)
        {
            if (id != analysisPannel.Id)
            {
                return NotFound();
            }


            analysisPannel.Pannel = _context.Pannel.Where(d => d.Id == pannelId).FirstOrDefault();
            analysisPannel.DataDisplay = _context.DataDisplay.Where(d => d.Id == dataDisplayId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(analysisPannel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnalysisPannelExists(analysisPannel.Id))
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
            return View(analysisPannel);
        }

        // GET: AnalysisPannels/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysisPannel = await _context.AnalysisPannel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (analysisPannel == null)
            {
                return NotFound();
            }

            return View(analysisPannel);
        }

        // POST: AnalysisPannels/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var analysisPannel = await _context.AnalysisPannel.FindAsync(id);
            _context.AnalysisPannel.Remove(analysisPannel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnalysisPannelExists(Guid id)
        {
            return _context.AnalysisPannel.Any(e => e.Id == id);
        }
    }
}
