using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ozo.Models;

namespace ozo.Controllers
{
    public class RadnikController : Controller
    {
        private readonly PI01Context _context;
        private readonly AppSettings appData;

        public RadnikController(PI01Context context, IOptions<AppSettings> options)
        {
            _context = context;
            appData = options.Value;
        }

        // GET: Radnik
        public async Task<IActionResult> Index()
        {
            var pI01Context = _context.Radnik.Include(r => r.Kategorija).Include(r => r.Osoba);
            return View(await pI01Context.ToListAsync());
        }

        // GET: Radnik/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var radnik = await _context.Radnik
                .Include(r => r.Kategorija)
                .Include(r => r.Osoba)
                .FirstOrDefaultAsync(m => m.RadnikId == id);
            if (radnik == null)
            {
                return NotFound();
            }

            return View(radnik);
        }

        public IActionResult Create(int osobaId, int zanimanjeId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Radnik radnik = new Radnik();
                    radnik.OsobaId = osobaId;
                    radnik.KategorijaId = zanimanjeId;


                    _context.Radnik.Add(radnik);
                    _context.SaveChanges();
                    //logger.LogInformation($"Osoba {osoba.Ime} dodana.");
                    // TempData[Constants.Message] = $"Osoba {} dodana.";
                    //  TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction("Index", "Osoba");

                }
                catch (Exception)
                {
                    //  logger.LogError("Pogreška prilikom dodavanje nove osobe: {0}", exc.CompleteExceptionMessage());
                    //  ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View();
                }
            }
            else
            {
                //PrepareDropDownLists();
                return View();
            }
        }





        // POST: Radnik/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        public async Task<IActionResult> EditPost(int osobaId, int zanimanjeId)
        {


            Console.WriteLine("mag" + osobaId);
            if (osobaId == null)
            {
                return NotFound();
            }

            var courseToUpdate = await _context.Radnik
                .FirstOrDefaultAsync(c => c.OsobaId == osobaId);
            Console.WriteLine("kita" + courseToUpdate.KategorijaId);
            
                
                try
                {
                   
                courseToUpdate.KategorijaId = zanimanjeId;
                await _context.SaveChangesAsync();

                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Neuspješno ažuriranje! ");
                }
                Console.WriteLine("mag" + zanimanjeId);
                return RedirectToAction("Index", "Osoba");
            
            
        }

        // GET: Radnik/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var radnik = await _context.Radnik
                .Include(r => r.Kategorija)
                .Include(r => r.Osoba)
                .FirstOrDefaultAsync(m => m.RadnikId == id);
            if (radnik == null)
            {
                return NotFound();
            }

            return View(radnik);
        }

        // POST: Radnik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var radnik = await _context.Radnik.FindAsync(id);
            _context.Radnik.Remove(radnik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RadnikExists(int id)
        {
            return _context.Radnik.Any(e => e.RadnikId == id);
        }
    }
}
