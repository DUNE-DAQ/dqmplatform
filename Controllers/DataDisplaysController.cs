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
using Microsoft.EntityFrameworkCore.Internal;

namespace DuneDaqMonitoringPlatform.Controllers
{
    public class DataDisplaysController : Controller
    {
        private readonly MonitoringDbContext _context;

        public DataDisplaysController(MonitoringDbContext context)
        {
            _context = context;
        }

        // GET: DataDisplays
        public async Task<IActionResult> Index()
        {

            return View(await _context.DataDisplay
                .Include(d => d.SamplingProfile)
                .Include(d => d.DataType).ToListAsync());
        }

        // GET: DataDisplays/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataDisplay = await _context.DataDisplay.Include(d => d.SamplingProfile).Include(d => d.DataType).FirstOrDefaultAsync(m => m.Id == id);
            if (dataDisplay == null)
            {
                return NotFound();
            }
            PopulateDataDisplayData(dataDisplay);

            return View(dataDisplay);
        }

        // GET: DataDisplays/Create
        public IActionResult Create()
        {
            ViewBag.Datas = _context.Data.ToList();
            ViewBag.DataTypes = _context.DataType.ToList();
            ViewBag.SamplingProfiles = _context.SamplingProfile.ToList();
            ViewBag.Analyses = _context.Analyse.ToList();

            PopulateDataDisplayData(new DataDisplay());

            return View();
        }

        // POST: DataDisplays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlotLength,Name")] DataDisplay dataDisplay, string[] multiSelector, Guid dataId, Guid dataTypeId, Guid samplingProfileId, Guid analyseId)
        {
            
            dataDisplay.DataType = _context.DataType.Where(d => d.Id == dataTypeId).FirstOrDefault();
            dataDisplay.SamplingProfile = _context.SamplingProfile.Where(d => d.Id == samplingProfileId).FirstOrDefault();


            if (ModelState.IsValid)
            {
                dataDisplay.Id = Guid.NewGuid();
                _context.Add(dataDisplay);
                EditDataDisplayData(dataDisplay, multiSelector.Select(Guid.Parse).ToList());
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dataDisplay);
        }

        
        private void PopulateDataDisplayData(DataDisplay dataDisplay)
        {
            List<Models.Data> datas = _context.Data.Include(d => d.DataDisplayDatas).Where(dd => dd.Name != "").ToList();

            var viewModel = new List<AssignedDataDisplayData>();
            foreach (var data in datas)
            {
                if (dataDisplay.DataDisplayDatas != null)
                {
                    viewModel.Add(new AssignedDataDisplayData
                    {
                        DataDisplayDataId = data.Id,
                        DisplayName = data.Name,
                        Assigned = (dataDisplay.DataDisplayDatas.Intersect(data.DataDisplayDatas).Count() != 0)
                    });
                }
                else 
                {
                    viewModel.Add(new AssignedDataDisplayData
                    {
                        DataDisplayDataId = data.Id,
                        DisplayName = data.Name
                    });
                }
            }
            ViewBag.Datas = viewModel;
        }

        private void EditDataDisplayData(DataDisplay dataDisplay, List<Guid> selectedDatas)
        {

            //List of existing relations
            var savedDataDisplayDatas = _context.DataDisplayData.Include(ddd => ddd.DataDisplay).Where(ddd => ddd.DataDisplay == dataDisplay).Include(ddd => ddd.Data).ToList();
            //Removes if exists but not selected
            foreach (DataDisplayData savedDataDisplayData in savedDataDisplayDatas)
            {
                //If saved GUID is not selected, deletes it
                if (!selectedDatas.Contains(savedDataDisplayData.Data.Id))
                {
                    _context.DataDisplayData.Remove(savedDataDisplayData);
                }
            }

            //add if doesn't exist
            foreach (Guid selectedData in selectedDatas)
            {
                int selectedDataDisplayAmount = _context.DataDisplayData.Include(ddd => ddd.DataDisplay).Include(ap => ap.Data).Where(ap => ap.Data.Id == selectedData).Where(ddd => ddd.DataDisplay == dataDisplay).Count();

                //If pannel does not exist
                if (selectedDataDisplayAmount == 0)
                {
                    DataDisplayData dataDisplayData = new DataDisplayData();
                    dataDisplayData.DataDisplay = dataDisplay;
                    dataDisplayData.Data = _context.Data.Where(d => d.Id == selectedData).FirstOrDefault();
                    dataDisplayData.Id = Guid.NewGuid();
                    _context.Add(dataDisplayData);
                }
            }
        }

        // GET: DataDisplays/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataDisplay = _context.DataDisplay.Include(dd => dd.DataDisplayDatas).Where(m => m.Id == id).FirstOrDefault();

            if (dataDisplay == null)
            {
                return NotFound();
            }

            ViewBag.DataTypes = _context.DataType.ToList();
            ViewBag.SamplingProfiles = _context.SamplingProfile.ToList();
            ViewBag.Analyses = _context.Analyse.ToList();

            PopulateDataDisplayData(dataDisplay);

            return View(dataDisplay);
        }

        // POST: DataDisplays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,PlotLength,Name")] DataDisplay dataDisplay, string[] multiSelector, Guid dataTypeId, Guid samplingProfileId, Guid analyseId)
        {
            if (id != dataDisplay.Id)
            {
                return NotFound();
            }

            dataDisplay.DataType = _context.DataType.Where(d => d.Id == dataTypeId).FirstOrDefault();
            dataDisplay.SamplingProfile = _context.SamplingProfile.Where(d => d.Id == samplingProfileId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dataDisplay);
                    EditDataDisplayData(dataDisplay, multiSelector.Select(Guid.Parse).ToList());
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataDisplayExists(dataDisplay.Id))
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
            return View(dataDisplay);
        }

        // GET: DataDisplays/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataDisplay = await _context.DataDisplay
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataDisplay == null)
            {
                return NotFound();
            }

            return View(dataDisplay);
        }

        // POST: DataDisplays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var dataDisplay = await _context.DataDisplay.FindAsync(id);
            _context.DataDisplay.Remove(dataDisplay);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DataDisplayExists(Guid id)
        {
            return _context.DataDisplay.Any(e => e.Id == id);
        }
    }
}
