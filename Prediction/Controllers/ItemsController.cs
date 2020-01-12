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

namespace Prediction.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ItemContext _context;

        public ItemsController(ItemContext context)
        {
            _context = context;
        }

        public IActionResult Line()
        {
            List<Item> items = _context.Items.ToList();
            TimeSeriesPrediction forecast = new TimeSeriesPrediction(items, Timeframe.Monthly);
            forecast.GenerateFutureForecast(12);

            var lstModel = new List<ChartViewModel>();
            var lstModel2 = new List<ChartViewModel>();
            var lstModel3 = new List<ChartViewModel>();
            foreach(Phone p in forecast.PhoneCollection.Phones)
            {
                lstModel.Add(new ChartViewModel
                {
                    Date = p.Date.ToString(),
                    Price = p.Forecast.Value
                });
                lstModel2.Add(new ChartViewModel
                {
                    Date = p.Date.ToString(),
                    Price = p.Forecast.Value + 1
                });
                lstModel3.Add(new ChartViewModel
                {
                    Date = p.Date.ToString(),
                    Price = p.Forecast.Value + 2
                });
            }

            List<List<ChartViewModel>> list = new List<List<ChartViewModel>>();
            list.Add(lstModel);
            list.Add(lstModel2);
            list.Add(lstModel3);

            var stackedview = new StackedViewModel()
            {
                StackedDimensionOne = "ASD",
                LstData = list
            };

            return View(list);
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            List<Item> items = _context.Items.ToList();
            TimeSeriesPrediction forecast = new TimeSeriesPrediction(items, Timeframe.Monthly);
            forecast.GenerateFutureForecast(12);
            ViewData["Phones"] = forecast.Print();
            return View(await _context.Items.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
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
                _context.Add(item);
                await _context.SaveChangesAsync();
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

            var item = await _context.Items.FindAsync(id);
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
                    _context.Update(item);
                    await _context.SaveChangesAsync();
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

            var item = await _context.Items
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
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }
    }
}
