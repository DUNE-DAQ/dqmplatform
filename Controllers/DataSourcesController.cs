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
            var dataSourceList = await _context.DataSources.ToListAsync();

            bool[] createdPannel = Enumerable.Repeat(false, dataSourceList.Count()).ToArray();
            int i = 0;
            foreach (DataSource dataSource in dataSourceList)
            {
                if (await _context.Pannel.Where(p => p.Name == dataSource.Source + " default panel").CountAsync() >= 1)
                {
                    createdPannel[i] = true;
                }
                i++;
            }

            ViewBag.CreatedPannel = createdPannel;
            // ViewBag.AllreadyGeneratedPannels 
            return View(dataSourceList);
        }

        //Made to create a standart pannel from a datasource
        public async Task<IActionResult> Pannel(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataSource = await _context.DataSources.FirstOrDefaultAsync(m => m.Id == id);
            var datas = await _context.Data.Where(d => d.DataSource == dataSource).ToListAsync();

            Pannel pannel = new Pannel();
            pannel.Id = Guid.NewGuid();
            pannel.Name = dataSource.Source + " default panel";
            pannel.Description = "Auto generated panel";
            _context.Add(pannel);


            foreach (Models.Data data in datas.Where(d => d.Name.Contains("fft_sums") && d.Name.Contains("plane") && !d.Name.Contains("plane_3")))
            {
                CreateDataDisplay(dataSource, data, pannel, "lines", "log", "Default");
            }
            foreach (Models.Data data in datas.Where(d => d.Name.Contains("raw") && d.Name.Contains("plane")))
            {
                CreateDataDisplay(dataSource, data, pannel, "heatmap", "standard", "Default");
            }
            foreach (Models.Data data in datas.Where(d => d.Name.Contains("rmsm") && d.Name.Contains("plane")))
            {
                CreateDataDisplay(dataSource, data, pannel, "markers", "standard", "Default");
            }

            return RedirectToAction("Index", "Pannels");
        }

        public void CreateDataDisplay(DataSource dataSource, Models.Data data, Pannel pannel, string plottignName, string plottignType, string samplingProfile)
        {
            DataDisplay dataDisplay = new DataDisplay();
            dataDisplay.Id = Guid.NewGuid();
            dataDisplay.DataType = _context.DataType.Where(d => d.PlottingType == plottignType && d.Name == plottignName).First();
            dataDisplay.SamplingProfile = _context.SamplingProfile.Where(d => d.Name == samplingProfile).First();
            dataDisplay.Name = dataSource.Source + " " + data.Name;
            dataDisplay.PlotLength = 0;

            //Create the intermediate table between datas and displays
            DataDisplayData dataDisplayData = new DataDisplayData();
            dataDisplayData.Id = Guid.NewGuid();
            dataDisplayData.Data = data;
            dataDisplayData.DataDisplay = dataDisplay;

            //Create the intermediate table between pannels and displays
            AnalysisPannel analysisPannel = new AnalysisPannel();
            analysisPannel.Id = Guid.NewGuid();
            analysisPannel.Pannel = pannel;
            analysisPannel.DataDisplay = dataDisplay;

            _context.Add(dataDisplayData);
            _context.Add(dataDisplay);
            _context.Add(analysisPannel);


            _context.SaveChanges();
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
