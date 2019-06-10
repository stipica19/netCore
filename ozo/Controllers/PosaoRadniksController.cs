using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ozo.Extensions;
using ozo.Models;

namespace ozo.Controllers
{
    public class PosaoRadniksController : Controller
    {
        private readonly PI01Context _context;

        public PosaoRadniksController(PI01Context context)
        {
            _context = context;
        }

        // GET: PosaoRadniks
        public async Task<IActionResult> Index()
        {
            var pI01Context = _context.PosaoRadnik.Include(p => p.Posao).Include(p => p.Radnik);
            return View(await pI01Context.ToListAsync());
        }

        // GET: PosaoRadniks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posaoRadnik = await _context.PosaoRadnik
                .Include(p => p.Posao)
                .Include(p => p.Radnik)
                .FirstOrDefaultAsync(m => m.RadnikPosaoId == id);
            if (posaoRadnik == null)
            {
                return NotFound();
            }

            return View(posaoRadnik);
        }

        public IActionResult Create(int posaoId, int kategorijaId, int opremaId)
        {

            if (ModelState.IsValid)
            {
               
                try
                {
                    PosaoRadnik pr = new PosaoRadnik();
                    pr.PosaoId = posaoId;
                   

                    Console.WriteLine("banana" + kategorijaId);
                    var x = _context.Radnik
                         .AsNoTracking()
                        .FromSql("SELECT * FROM dbo.Radnik")
                        .Where(c => c.KategorijaId == kategorijaId)
                        .Single();

                    pr.RadnikId = x.RadnikId;
                    _context.Add(pr);
                  
                    _context.SaveChanges();
                    
                    // return RedirectToAction("Index", "Oprema");
                    //logger.LogInformation($"Osoba {osoba.Ime} dodana.");
                    // TempData[Constants.Message] = $"Osoba {} dodana.";
                    //  TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction("Create", "PosaoOprema", new { posaoId, opremaId });

                }
                catch (Exception exc)
                {
                    //logger.LogError("Pogreška prilikom dodavanje nove osobe: {0}", exc.CompleteExceptionMessage());
                     ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View();
                }
            }
            else
            {
                //PrepareDropDownLists();
                return View();
            }

        }

        // GET: PosaoRadniks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posaoRadnik = await _context.PosaoRadnik.FindAsync(id);
            if (posaoRadnik == null)
            {
                return NotFound();
            }
            ViewData["PosaoId"] = new SelectList(_context.Posao, "PosaoId", "Opis", posaoRadnik.PosaoId);
            ViewData["RadnikId"] = new SelectList(_context.Radnik, "RadnikId", "RadnikId", posaoRadnik.RadnikId);
            return View(posaoRadnik);
        }

        // POST: PosaoRadniks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RadnikPosaoId,RadnikId,PosaoId")] PosaoRadnik posaoRadnik)
        {
            if (id != posaoRadnik.RadnikPosaoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(posaoRadnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PosaoRadnikExists(posaoRadnik.RadnikPosaoId))
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
            ViewData["PosaoId"] = new SelectList(_context.Posao, "PosaoId", "Opis", posaoRadnik.PosaoId);
            ViewData["RadnikId"] = new SelectList(_context.Radnik, "RadnikId", "RadnikId", posaoRadnik.RadnikId);
            return View(posaoRadnik);
        }

        // GET: PosaoRadniks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posaoRadnik = await _context.PosaoRadnik
                .Include(p => p.Posao)
                .Include(p => p.Radnik)
                .FirstOrDefaultAsync(m => m.RadnikPosaoId == id);
            if (posaoRadnik == null)
            {
                return NotFound();
            }

            return View(posaoRadnik);
        }

        // POST: PosaoRadniks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var posaoRadnik = await _context.PosaoRadnik.FindAsync(id);
            _context.PosaoRadnik.Remove(posaoRadnik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PosaoRadnikExists(int id)
        {
            return _context.PosaoRadnik.Any(e => e.RadnikPosaoId == id);
        }
    }
}
