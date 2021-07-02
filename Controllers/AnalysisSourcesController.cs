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
    public class AnalysisSourcesController : Controller
    {
        private readonly MonitoringDbContext _context;

        public AnalysisSourcesController(MonitoringDbContext context)
        {
            _context = context;
        }

        // GET: AnalysisSources
        public async Task<IActionResult> Index()
        {
            return View(await _context.AnalysisSource.ToListAsync());
        }

        // GET: AnalysisSources/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysisSource = await _context.AnalysisSource
                .FirstOrDefaultAsync(m => m.Id == id);
            if (analysisSource == null)
            {
                return NotFound();
            }

            return View(analysisSource);
        }

        // GET: AnalysisSources/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: AnalysisSources/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] AnalysisSource analysisSource)
        {
            if (ModelState.IsValid)
            {
                analysisSource.Id = Guid.NewGuid();
                _context.Add(analysisSource);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(analysisSource);
        }

        // GET: AnalysisSources/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysisSource = await _context.AnalysisSource.FindAsync(id);
            if (analysisSource == null)
            {
                return NotFound();
            }
            return View(analysisSource);
        }

        // POST: AnalysisSources/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description")] AnalysisSource analysisSource)
        {
            if (id != analysisSource.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(analysisSource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnalysisSourceExists(analysisSource.Id))
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
            return View(analysisSource);
        }

        // GET: AnalysisSources/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysisSource = await _context.AnalysisSource
                .FirstOrDefaultAsync(m => m.Id == id);
            if (analysisSource == null)
            {
                return NotFound();
            }

            return View(analysisSource);
        }

        // POST: AnalysisSources/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var analysisSource = await _context.AnalysisSource.FindAsync(id);
            _context.AnalysisSource.Remove(analysisSource);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnalysisSourceExists(Guid id)
        {
            return _context.AnalysisSource.Any(e => e.Id == id);
        }
    }
}
