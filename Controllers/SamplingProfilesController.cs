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
    public class SamplingProfilesController : Controller
    {
        private readonly MonitoringDbContext _context;

        public SamplingProfilesController(MonitoringDbContext context)
        {
            _context = context;
        }

        // GET: SamplingProfiles
        public async Task<IActionResult> Index()
        {
            return View(await _context.SamplingProfile.ToListAsync());
        }

        // GET: SamplingProfiles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var samplingProfile = await _context.SamplingProfile
                .FirstOrDefaultAsync(m => m.Id == id);
            if (samplingProfile == null)
            {
                return NotFound();
            }

            return View(samplingProfile);
        }

        // GET: SamplingProfiles/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: SamplingProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,Name,PlottingType,Description,Factor")] SamplingProfile samplingProfile)
        {
            if (ModelState.IsValid)
            {
                samplingProfile.Id = Guid.NewGuid();
                _context.Add(samplingProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(samplingProfile);
        }

        // GET: SamplingProfiles/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var samplingProfile = await _context.SamplingProfile.FindAsync(id);
            if (samplingProfile == null)
            {
                return NotFound();
            }
            return View(samplingProfile);
        }

        // POST: SamplingProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,PlottingType,Description,Factor")] SamplingProfile samplingProfile)
        {
            if (id != samplingProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(samplingProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SamplingProfileExists(samplingProfile.Id))
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
            return View(samplingProfile);
        }

        // GET: SamplingProfiles/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var samplingProfile = await _context.SamplingProfile
                .FirstOrDefaultAsync(m => m.Id == id);
            if (samplingProfile == null)
            {
                return NotFound();
            }

            return View(samplingProfile);
        }

        // POST: SamplingProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var samplingProfile = await _context.SamplingProfile.FindAsync(id);
            _context.SamplingProfile.Remove(samplingProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SamplingProfileExists(Guid id)
        {
            return _context.SamplingProfile.Any(e => e.Id == id);
        }
    }
}
