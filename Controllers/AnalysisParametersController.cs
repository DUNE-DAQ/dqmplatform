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
    public class AnalysisParametersController : Controller
    {
        private readonly MonitoringDbContext _context;

        public AnalysisParametersController(MonitoringDbContext context)
        {
            _context = context;
        }

        // GET: AnalysisParameters
        public async Task<IActionResult> Index()
        {
            return View(await _context.AnalysisParameter.Include(d => d.Analyse).Include(d => d.Parameter).ToListAsync());
        }

        // GET: AnalysisParameters/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysisParameter = await _context.AnalysisParameter.Include(d => d.Analyse).Include(d => d.Parameter).FirstOrDefaultAsync(m => m.Id == id);
            if (analysisParameter == null)
            {
                return NotFound();
            }

            return View(analysisParameter);
        }

        // GET: AnalysisParameters/Create
        public IActionResult Create()
        {
            ViewBag.Analyses = _context.Analyse.ToList();
            ViewBag.Parameters = _context.Parameter.ToList();
            return View();
        }

        // POST: AnalysisParameters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Degree,Interval,Type")] AnalysisParameter analysisParameter, Guid analyseId, Guid parameterId)
        {
            analysisParameter.Analyse = _context.Analyse.Where(d => d.Id == analyseId).FirstOrDefault();
            analysisParameter.Parameter = _context.Parameter.Where(d => d.Id == parameterId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                analysisParameter.Id = Guid.NewGuid();
                _context.Add(analysisParameter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(analysisParameter);
        }

        // GET: AnalysisParameters/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysisParameter = await _context.AnalysisParameter.FindAsync(id);
            if (analysisParameter == null)
            {
                return NotFound();
            }

            ViewBag.Analyses = _context.Analyse.ToList();
            ViewBag.Parameters = _context.Parameter.ToList();

            return View(analysisParameter);
        }

        // POST: AnalysisParameters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Degree,Interval,Type")] AnalysisParameter analysisParameter, Guid analyseId, Guid parameterId)
        {

            analysisParameter.Analyse = _context.Analyse.Where(d => d.Id == analyseId).FirstOrDefault();
            analysisParameter.Parameter = _context.Parameter.Where(d => d.Id == parameterId).FirstOrDefault();

            if (id != analysisParameter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(analysisParameter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnalysisParameterExists(analysisParameter.Id))
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
            return View(analysisParameter);
        }

        // GET: AnalysisParameters/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysisParameter = await _context.AnalysisParameter
                .FirstOrDefaultAsync(m => m.Id == id);
            if (analysisParameter == null)
            {
                return NotFound();
            }

            return View(analysisParameter);
        }

        // POST: AnalysisParameters/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var analysisParameter = await _context.AnalysisParameter.FindAsync(id);
            _context.AnalysisParameter.Remove(analysisParameter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnalysisParameterExists(Guid id)
        {
            return _context.AnalysisParameter.Any(e => e.Id == id);
        }
    }
}
