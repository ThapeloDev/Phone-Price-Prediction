using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Prediction.Models;
using Prediction.Models.Enums;
using Prediction.Models.Hardware;
using Prediction.Models.Time_Series_Forecasting;
using Prediction.View_Models.Chart;
using Prediction.View_Models.Hardware_Suggestion;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        #region Forecast
        public IActionResult Chart(List<int> selectedItems = null, int forecastMonths = 12)
        {
            List<Item> phones = _phoneContext.Items.ToList();
            List<PhoneProperties> phoneInfo = _hardwareContext.PhoneProperties.ToList();

            if (selectedItems == null)
            {
                return View(new ForecastChart(phones, phoneInfo, forecastMonths));
            }
            else
            {
                ForecastChart existingModel = new ForecastChart(phones, phoneInfo, forecastMonths, selectedItems);
                return View(existingModel);
            }
        }

        public IActionResult AddToChart(string forecastMonths, string currentItem, string selectedItems = null)
        {
            List<int> AllItems = new List<int>();
            if (selectedItems != null)
            {
                var SelectedItems = JsonConvert.DeserializeObject<List<int>>(selectedItems);
                AllItems.AddRange(SelectedItems);
            }
            int SelectedId = JsonConvert.DeserializeObject<int>(currentItem);
            AllItems.Add(SelectedId);

            int forecast = JsonConvert.DeserializeObject<int>(forecastMonths);

            return RedirectToAction("Chart", "Items", new { selectedItems = AllItems, forecastMonths = forecast });
        }

        public IActionResult RemoveFromChart(string currentItem, string selectedItems, string forecastMonths)
        {
            List<int> AllItems = new List<int>();

            var SelectedItems = JsonConvert.DeserializeObject<List<int>>(selectedItems);
            AllItems.AddRange(SelectedItems);

            int SelectedId = JsonConvert.DeserializeObject<int>(currentItem);
            if (AllItems.Contains(SelectedId))
            {
                AllItems.RemoveAll(x => x == SelectedId);
            }

            int forecast = JsonConvert.DeserializeObject<int>(forecastMonths);

            return RedirectToAction("Chart", "Items", new { selectedItems = AllItems, forecastMonths = forecast });
        }

        [HttpPost]
        public IActionResult ChangeForecastMonths(string selectedItems, string forecastMonths)
        {
            List<int> allIteams = JsonConvert.DeserializeObject<List<int>>(selectedItems);
            int forecast = JsonConvert.DeserializeObject<int>(forecastMonths);

            return RedirectToAction("Chart", "Items", new { selectedItems = allIteams, forecastMonths = forecast });
        }
        #endregion

        #region Phone Info
        public IActionResult Info(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<Item> transactions = _phoneContext.Items.ToList();
            List<PhoneProperties> hardware = _hardwareContext.PhoneProperties.ToList();
            HistoricalChart model = new HistoricalChart(id.Value, transactions, hardware);

            return View(model);
        }
        #endregion

        public IActionResult HardwareCalc(string id = null, int forecastMonths = 12)
        {
            List<Item> transactions = _phoneContext.Items.ToList();
            List<PhoneProperties> hardware = _hardwareContext.PhoneProperties.ToList();
            List<int> eligibleIds = new List<int>();

            if (id != null)
            {
                eligibleIds = JsonConvert.DeserializeObject<List<int>>(id);
            }

            return View(new PurchaseSuggestion(transactions, hardware, eligibleIds));
        }

        public IActionResult ChangeSpecs(int? storage, bool? hasMemoryCardReader, int? cpuCoreCount, double? cpuSpeed,
            int? ram, bool? headphoneOutput, bool? is5gCapable, int? frontCameraMgpx, int? backCameraMgpx,
            int? rearCameraCount, bool? exchangableBattery, bool? wirelessCharging, bool? fastCharging, bool? waterResistance)
        {
            List<PhoneProperties> eligiblePhones = new List<PhoneProperties>();
            eligiblePhones = _hardwareContext.PhoneProperties.Where(x => (x.HasMemoryCardReader == hasMemoryCardReader || x.HasMemoryCardReader == true) &&
                                                                         (x.HeadphoneOutput == headphoneOutput || x.HeadphoneOutput == true)  &&
                                                                         (x.Is5gCapable == is5gCapable || x.Is5gCapable == true) &&
                                                                         (x.ExchangableBattery == exchangableBattery || x.ExchangableBattery == true) &&
                                                                         (x.WirelessCharging == wirelessCharging || x.WirelessCharging == true) &&
                                                                         (x.FastCharging == fastCharging || x.FastCharging == true) &&
                                                                         (x.WaterResistance == waterResistance || x.WaterResistance == true)).ToList();
            if (storage.HasValue)
                eligiblePhones = _hardwareContext.PhoneProperties.Where(x => x.Storage >= storage).ToList();
            if (cpuCoreCount.HasValue)
                eligiblePhones = _hardwareContext.PhoneProperties.Where(x => x.CpuCoreCount >= cpuCoreCount).ToList();
            if (cpuSpeed.HasValue)
                eligiblePhones = _hardwareContext.PhoneProperties.Where(x => x.CpuSpeed >= cpuSpeed).ToList();
            if (ram.HasValue)
                eligiblePhones = _hardwareContext.PhoneProperties.Where(x => x.RAM >= ram).ToList();
            if (frontCameraMgpx.HasValue)
                eligiblePhones = _hardwareContext.PhoneProperties.Where(x => x.FrontCameraMegapixel >= frontCameraMgpx).ToList();
            if (backCameraMgpx.HasValue)
                eligiblePhones = _hardwareContext.PhoneProperties.Where(x => x.BackCameraMegapixel >= backCameraMgpx).ToList();
            if (rearCameraCount.HasValue)
                eligiblePhones = _hardwareContext.PhoneProperties.Where(x => x.RearCameraCount >= rearCameraCount).ToList();

            List<int> eligibleIds = eligiblePhones.Select(x => x.ConfigId).ToList(); 
            string serializedIds = JsonConvert.SerializeObject(eligibleIds);

            return RedirectToAction("HardwareCalc", "Items", new { id = serializedIds });
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

        public IActionResult RedirectToNotFound()
        {
            return NotFound();
        }
    }
}
