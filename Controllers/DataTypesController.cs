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
    public class DataTypesController : Controller
    {
        private readonly MonitoringDbContext _context;

        public DataTypesController(MonitoringDbContext context)
        {
            _context = context;
        }

        // GET: DataTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.DataType.ToListAsync());
        }

        // GET: DataTypes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataType = await _context.DataType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataType == null)
            {
                return NotFound();
            }

            return View(dataType);
        }

        // GET: DataTypes/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: DataTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,Name,PlottingType,Description")] DataType dataType)
        {
            if (ModelState.IsValid)
            {
                dataType.Id = Guid.NewGuid();
                _context.Add(dataType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dataType);
        }

        // GET: DataTypes/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataType = await _context.DataType.FindAsync(id);
            if (dataType == null)
            {
                return NotFound();
            }
            return View(dataType);
        }

        // POST: DataTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,PlottingType,Description")] DataType dataType)
        {
            if (id != dataType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dataType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataTypeExists(dataType.Id))
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
            return View(dataType);
        }

        // GET: DataTypes/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataType = await _context.DataType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataType == null)
            {
                return NotFound();
            }

            return View(dataType);
        }

        // POST: DataTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var dataType = await _context.DataType.FindAsync(id);
            _context.DataType.Remove(dataType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DataTypeExists(Guid id)
        {
            return _context.DataType.Any(e => e.Id == id);
        }
    }
}
