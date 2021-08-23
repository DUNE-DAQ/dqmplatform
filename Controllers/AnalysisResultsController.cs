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
    public class AnalysisResultsController : Controller
    {
        private readonly MonitoringDbContext _context;

        public AnalysisResultsController(MonitoringDbContext context)
        {
            _context = context;
        }

        // GET: AnalysisResults
        public async Task<IActionResult> Index()
        {
            return View(await _context.AnalysisResult.Include(d => d.AnalysisParameter).Include(d => d.AnalysisParameter.Analyse).Include(d => d.DataPath).Include(d => d.DataPath.Data).ToListAsync());
        }

        // GET: AnalysisResults/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysisResult = await _context.AnalysisResult.Include(d => d.AnalysisParameter.Analyse).Include(d => d.DataPath.Data).FirstOrDefaultAsync(m => m.Id == id);
            if (analysisResult == null)
            {
                return NotFound();
            }

            return View(analysisResult);
        }

        // GET: AnalysisResults/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: AnalysisResults/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Decision,Confidence")] AnalysisResult analysisResult)
        {
            if (ModelState.IsValid)
            {
                analysisResult.Id = Guid.NewGuid();
                _context.Add(analysisResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(analysisResult);
        }

        // GET: AnalysisResults/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysisResult = await _context.AnalysisResult.FindAsync(id);
            if (analysisResult == null)
            {
                return NotFound();
            }
            return View(analysisResult);
        }

        // POST: AnalysisResults/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Decision,Confidence")] AnalysisResult analysisResult)
        {
            if (id != analysisResult.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(analysisResult);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnalysisResultExists(analysisResult.Id))
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
            return View(analysisResult);
        }

        // GET: AnalysisResults/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysisResult = await _context.AnalysisResult
                .FirstOrDefaultAsync(m => m.Id == id);
            if (analysisResult == null)
            {
                return NotFound();
            }

            return View(analysisResult);
        }

        // POST: AnalysisResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var analysisResult = await _context.AnalysisResult.FindAsync(id);
            _context.AnalysisResult.Remove(analysisResult);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnalysisResultExists(Guid id)
        {
            return _context.AnalysisResult.Any(e => e.Id == id);
        }
    }
}
