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
    public class AnalysesController : Controller
    {
        private readonly MonitoringDbContext _context;

        public AnalysesController(MonitoringDbContext context)
        {
            _context = context;
        }

        // GET: Analyses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Analyse.Include(d => d.Data).Include(d => d.AnalysisSource).ToListAsync());
        }

        // GET: Analyses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analyse = await _context.Analyse.Include(d => d.Data).Include(d => d.AnalysisSource).FirstOrDefaultAsync(m => m.Id == id);
            if (analyse == null)
            {
                return NotFound();
            }

            return View(analyse);
        }

        // GET: Analyses/Create
        public IActionResult Create()
        {
            ViewBag.Datas = _context.Data.ToList();
            ViewBag.AnalysisSources = _context.AnalysisSource.ToList();

            return View();
        }

        // POST: Analyses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Running")] Analyse analyse, Guid analysisSourcesId, Guid dataId)
        {

            analyse.Data = _context.Data.Where(d => d.Id == dataId).FirstOrDefault();
            analyse.AnalysisSource = _context.AnalysisSource.Where(d => d.Id == analysisSourcesId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                analyse.Id = Guid.NewGuid();
                _context.Add(analyse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(analyse);
        }

        // GET: Analyses/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analyse = await _context.Analyse.FindAsync(id);
            if (analyse == null)
            {
                return NotFound();
            }


            ViewBag.Datas = _context.Data.ToList();
            ViewBag.AnalysisSources = _context.AnalysisSource.ToList();


            return View(analyse);
        }

        // POST: Analyses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,Running")] Analyse analyse, Guid analysisSourcesId, Guid dataId)
        {
            if (id != analyse.Id)
            {
                return NotFound();
            }

            analyse.Data = _context.Data.Where(d => d.Id == dataId).FirstOrDefault();
            analyse.AnalysisSource = _context.AnalysisSource.Where(d => d.Id == analysisSourcesId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(analyse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnalyseExists(analyse.Id))
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
            return View(analyse);
        }

        // GET: Analyses/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analyse = await _context.Analyse
                .FirstOrDefaultAsync(m => m.Id == id);
            if (analyse == null)
            {
                return NotFound();
            }

            return View(analyse);
        }

        // POST: Analyses/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var analyse = await _context.Analyse.FindAsync(id);
            _context.Analyse.Remove(analyse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnalyseExists(Guid id)
        {
            return _context.Analyse.Any(e => e.Id == id);
        }
    }
}
