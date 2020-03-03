using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prediction.Models.Hardware;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction.Controllers
{
    public class PhonePropertiesController : Controller
    {
        private readonly PhonePropertiesContext _context;

        public PhonePropertiesController(PhonePropertiesContext context)
        {
            _context = context;
        }

        // GET: PhoneProperties
        public async Task<IActionResult> Index()
        {
            return View(await _context.PhoneProperties.ToListAsync());
        }

        // GET: PhoneProperties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phoneProperties = await _context.PhoneProperties
                .FirstOrDefaultAsync(m => m.ConfigId == id);
            if (phoneProperties == null)
            {
                return NotFound();
            }

            return View(phoneProperties);
        }

        // GET: PhoneProperties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PhoneProperties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConfigId,Brand,Model,Storage,Cpu,CpuType,CpuSpeed,RAM,HasGPU,HeadphoneOutput,Capable5g,FrontCameraMegapixel,BackCameraMegapixel,BatteryCapacity,ExchangableBattery,Depth,Height,Width,Weight,WirelessCharging,WirelessStandard,DualSim,SimCard,FastCharging,WaterResistance,ReleaseYear")] PhoneProperties phoneProperties)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phoneProperties);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(phoneProperties);
        }

        // GET: PhoneProperties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phoneProperties = await _context.PhoneProperties.FindAsync(id);
            if (phoneProperties == null)
            {
                return NotFound();
            }
            return View(phoneProperties);
        }

        // POST: PhoneProperties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConfigId,Brand,Model,Storage,Cpu,CpuType,CpuSpeed,RAM,HasGPU,HeadphoneOutput,Capable5g,FrontCameraMegapixel,BackCameraMegapixel,BatteryCapacity,ExchangableBattery,Depth,Height,Width,Weight,WirelessCharging,WirelessStandard,DualSim,SimCard,FastCharging,WaterResistance,ReleaseYear")] PhoneProperties phoneProperties)
        {
            if (id != phoneProperties.ConfigId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phoneProperties);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhonePropertiesExists(phoneProperties.ConfigId))
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
            return View(phoneProperties);
        }

        // GET: PhoneProperties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phoneProperties = await _context.PhoneProperties
                .FirstOrDefaultAsync(m => m.ConfigId == id);
            if (phoneProperties == null)
            {
                return NotFound();
            }

            return View(phoneProperties);
        }

        // POST: PhoneProperties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phoneProperties = await _context.PhoneProperties.FindAsync(id);
            _context.PhoneProperties.Remove(phoneProperties);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhonePropertiesExists(int id)
        {
            return _context.PhoneProperties.Any(e => e.ConfigId == id);
        }
    }
}
