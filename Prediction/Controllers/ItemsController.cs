using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Prediction.Models;
using Prediction.Models.Chart;
using Prediction.Models.Enums;
using Prediction.Models.Time_Series_Forecasting;
using Prediction.Models.Time_Series_Forecasting.Cleaning;
using ChartJSCore.Models;
using ChartJSCore.Helpers;
using ChartJSCore.Plugins;
using System.Drawing;
using Prediction.Models.Chart.Misc;
using Prediction.Models.Hardware;
using Prediction.Models.ChartManual;
using Newtonsoft.Json;

namespace Prediction.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ItemContext _phoneContext;
        private readonly PhonePropertiesContext _hardwareContext;

        public ItemsController(ItemContext phoneContext, PhonePropertiesContext hardwareContext)
        {
            _phoneContext = phoneContext;
            _hardwareContext = hardwareContext;
        }

        public IActionResult Chart(List<int> selectedItems = null)
        {
            List<Item> phones = _phoneContext.Items.ToList();
            List<PhoneProperties> phoneInfo = _hardwareContext.PhoneProperties.ToList();


            if (selectedItems == null)
            {
                return View(new ManualChart(phones ,phoneInfo));
            }
            else
            {
                ManualChart existingModel = new ManualChart(phones ,phoneInfo, selectedItems);
                return View(existingModel);
            }
        }

        public IActionResult AddToChart(string currentItem, string selectedItems = null)
        {
            List<int> AllItems = new List<int>();
            if(selectedItems != null)
            {
                var SelectedItems = JsonConvert.DeserializeObject<List<int>>(selectedItems);
                AllItems.AddRange(SelectedItems);
            }
            int SelectedId = JsonConvert.DeserializeObject<int>(currentItem);
            AllItems.Add(SelectedId);

            return RedirectToAction("Chart", "Items", new { selectedItems = AllItems });
        }

        public IActionResult RemoveFromChart(string currentItem, string selectedItems)
        {
            List<int> AllItems = new List<int>();

            var SelectedItems = JsonConvert.DeserializeObject<List<int>>(selectedItems);
            AllItems.AddRange(SelectedItems);

            int SelectedId = JsonConvert.DeserializeObject<int>(currentItem);
            if (AllItems.Contains(SelectedId))
            {
                AllItems.RemoveAll(x => x == SelectedId);
            }

            return RedirectToAction("Chart", "Items", new { selectedItems = AllItems });
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            List<Item> items = _phoneContext.Items.ToList();
            TimeSeriesPrediction forecast = new TimeSeriesPrediction(items, Timeframe.Monthly);
            forecast.GenerateFutureForecast(12);
            return View(await _phoneContext.Items.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _phoneContext.Items
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,Brand,Model,Date,Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                _phoneContext.Add(item);
                await _phoneContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _phoneContext.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,Brand,Model,Date,Price")] Item item)
        {
            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _phoneContext.Update(item);
                    await _phoneContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemId))
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
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _phoneContext.Items
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _phoneContext.Items.FindAsync(id);
            _phoneContext.Items.Remove(item);
            await _phoneContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _phoneContext.Items.Any(e => e.ItemId == id);
        }
    }
}
