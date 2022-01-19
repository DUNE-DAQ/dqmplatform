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
using DuneDaqMonitoringPlatform.Services;

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
                DbItemCreation.CreateDataDisplay(_context, dataSource, data, pannel, "lines", "log", "Default");
            }
            foreach (Models.Data data in datas.Where(d => d.Name.Contains("raw") && d.Name.Contains("plane")))
            {
                DbItemCreation.CreateDataDisplay(_context, dataSource, data, pannel, "heatmap", "standard", "Default");
            }
            foreach (Models.Data data in datas.Where(d => d.Name.Contains("rmsm") && d.Name.Contains("plane")))
            {
                DbItemCreation.CreateDataDisplay(_context, dataSource, data, pannel, "markers", "standard", "Default");
            }

            return RedirectToAction("Index", "Pannels");
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
