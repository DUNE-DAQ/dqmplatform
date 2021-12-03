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
using DuneDaqMonitoringPlatform.ViewModels;

namespace DuneDaqMonitoringPlatform.Controllers
{
    [Authorize]
    public class PannelsController : Controller
    {
        private readonly MonitoringDbContext _context;

        public PannelsController(MonitoringDbContext context)
        {
            _context = context;
        }

        // GET: Pannels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pannel.ToListAsync());
        }

        // GET: Pannels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pannel = await _context.Pannel.FirstOrDefaultAsync(m => m.Id == id);
            if (pannel == null)
            {
                return NotFound();
            }

            ViewBag.DataDisplayIdList = await _context.AnalysisPannel.Where(ap => ap.Pannel.Id == pannel.Id).Include(p => p.DataDisplay).OrderBy(dd => dd.DataDisplay.Name).Select(ap => ap.DataDisplay.Id).ToListAsync();

            return View(pannel);
        }

        public async Task<IActionResult> DetailsStatic(Guid? id, string startTime)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pannel = await _context.Pannel.FirstOrDefaultAsync(m => m.Id == id);
            if (pannel == null)
            {
                return NotFound();
            }

            ViewBag.DataDisplayIdList = await _context.AnalysisPannel.Where(ap => ap.Pannel.Id == pannel.Id).Include(p => p.DataDisplay).OrderBy(dd => dd.DataDisplay.Name).Select(ap => ap.DataDisplay.Id).ToListAsync();
            ViewBag.Time = Convert.ToDateTime(startTime);


            return View(pannel);
        }

        [Authorize(Roles = "Administrator, User")]
        // GET: Pannels/Create
        public IActionResult Create()
        {
            ViewBag.DataDisplays = _context.DataDisplay.Where(dd => dd.Name != "").ToList();

            return View();
        }

        // POST: Pannels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Pannel pannel)
        {
            if (ModelState.IsValid)
            {
                pannel.Id = Guid.NewGuid();
                _context.Add(pannel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pannel);
        }

        // GET: Pannels/Edit/5
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pannel = _context.Pannel.Include(p => p.AnalysisPannels).Where(m => m.Id == id).FirstOrDefault();
            
            if (pannel == null)
            {
                return NotFound();
            }
            PopulateAnalysisPannels(pannel);
            return View(pannel);
        }

        private void PopulateAnalysisPannels(Pannel pannel)
        {
            var dataDisplays = _context.DataDisplay.Include(dd => dd.AnalysisPannels).Where(dd => dd.Name != "").ToList();
            
            var viewModel = new List<AssignedAnalysisPannel>();
            foreach (var dataDisplay in dataDisplays)
            {
                viewModel.Add(new AssignedAnalysisPannel
                {
                    AnalysisPannelId = dataDisplay.Id,
                    DisplayName = dataDisplay.Name,
                    Assigned = (pannel.AnalysisPannels.Intersect(dataDisplay.AnalysisPannels).Count() != 0)
                });
            }
            ViewBag.DataDisplays = viewModel;
        }

        [Authorize(Roles = "Administrator, User")]
        private void EditAnalysisPannels(Pannel pannel, List<Guid> selectedDataDisplays)
        {

            //List of existing relations
            List<AnalysisPannel> savedAnalysisPannels = _context.AnalysisPannel.Include(ap => ap.Pannel).Where(ap => ap.Pannel == pannel).Include(ap => ap.DataDisplay).ToList();
            //Removes if exists but not selected
            foreach (AnalysisPannel savedAnalysisPannel in savedAnalysisPannels)
            {
                //If saved GUID is not selected, deletes it
                if(!selectedDataDisplays.Contains(savedAnalysisPannel.DataDisplay.Id))
                {
                    _context.AnalysisPannel.Remove(savedAnalysisPannel);
                }
            }

            //add if doesn't exist
            foreach (Guid selectedDataDisplay in selectedDataDisplays)
            {
                int selectedPannelsAmount = _context.AnalysisPannel.Include(ap => ap.Pannel).Include(ap => ap.DataDisplay).Where(ap => ap.DataDisplay.Id == selectedDataDisplay).Where(ap => ap.Pannel == pannel).Count();

                //If pannel does not exist
                if (selectedPannelsAmount == 0)
                {

                    AnalysisPannel analysisPannel = new AnalysisPannel();
                    analysisPannel.Pannel = pannel;
                    analysisPannel.DataDisplay = _context.DataDisplay.Where(d => d.Id == selectedDataDisplay).FirstOrDefault();
                    analysisPannel.Id = Guid.NewGuid();
                    _context.Add(analysisPannel);

                    
                }
            }
        }

        // POST: Pannels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description")] Pannel pannel, string[] multiSelector)
        {
            if (id != pannel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pannel);
                    EditAnalysisPannels(pannel, multiSelector.Select(Guid.Parse).ToList());
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PannelExists(pannel.Id))
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
            return View(pannel);
        }

        // GET: Pannels/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pannel = await _context.Pannel.FirstOrDefaultAsync(m => m.Id == id);
            if (pannel == null)
            {
                return NotFound();
            }

            return View(pannel);
        }

        // POST: Pannels/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var pannel = await _context.Pannel.Include(p => p.AnalysisPannels).Where(p => p.Id == id).FirstOrDefaultAsync();
            //Remove displays related to this pannel only
            foreach(AnalysisPannel analysisPannel in pannel.AnalysisPannels)
            {
                //Selects the analysis pannel to remove
                var analysisPannelToRemove = await _context.AnalysisPannel.Include(ap => ap.DataDisplay).Include(dd => dd.DataDisplay.DataDisplayDatas).Where(ap => ap == analysisPannel).FirstOrDefaultAsync();
                //if it was the only analysis pannel containing this display, deletes the data display
                if (await _context.AnalysisPannel.Where(ap => ap.DataDisplay == analysisPannelToRemove.DataDisplay).CountAsync() == 1)
                {
                    foreach(DataDisplayData dataDisplayData in analysisPannelToRemove.DataDisplay.DataDisplayDatas)
                    {
                        _context.DataDisplayData.Remove(dataDisplayData);
                    }
                    _context.DataDisplay.Remove(analysisPannelToRemove.DataDisplay);
                }
                _context.AnalysisPannel.Remove(analysisPannel);
            }

            _context.Pannel.Remove(pannel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PannelExists(Guid id)
        {
            return _context.Pannel.Any(e => e.Id == id);
        }
    }
}
