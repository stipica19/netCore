using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ozo.Models;

namespace ozo.Controllers
{
    public class UslugaLjudiController : Controller
    {
        private readonly PI01Context _context;

        public UslugaLjudiController(PI01Context context)
        {
            _context = context;
        }

        // GET: UslugaLjudi
        public async Task<IActionResult> Index()
        {
            var pI01Context = _context.UslugaLjudi.Include(u => u.Usluga).Include(u => u.Zanimanje);
            return View(await pI01Context.ToListAsync());
        }

        // GET: UslugaLjudi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uslugaLjudi = await _context.UslugaLjudi
                .Include(u => u.Usluga)
                .Include(u => u.Zanimanje)
                .FirstOrDefaultAsync(m => m.UslugaLjudiId == id);
            if (uslugaLjudi == null)
            {
                return NotFound();
            }

            return View(uslugaLjudi);
        }

        // GET: UslugaLjudi/Create
        public IActionResult Create(int uslugaId, int zanimanjeId,int referentniTipOpremeId)
        {
            
            if (ModelState.IsValid)
            {
                
                try
                {
                    UslugaLjudi uslugaLjudi = new UslugaLjudi();
                    uslugaLjudi.UslugaId = uslugaId;
                    uslugaLjudi.ZanimanjeId = zanimanjeId;

                    
                    _context.UslugaLjudi.Add(uslugaLjudi);
                    _context.SaveChanges();
                    //logger.LogInformation($"Osoba {osoba.Ime} dodana.");
                    // TempData[Constants.Message] = $"Osoba {} dodana.";
                    //  TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction("Create", "UslugaOprema", new { uslugaId, referentniTipOpremeId });

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

        // GET: UslugaLjudi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uslugaLjudi = await _context.UslugaLjudi.FindAsync(id);
            if (uslugaLjudi == null)
            {
                return NotFound();
            }
            ViewData["UslugaId"] = new SelectList(_context.Usluga, "UslugaId", "Naziv", uslugaLjudi.UslugaId);
            ViewData["ZanimanjeId"] = new SelectList(_context.Zanimanje, "ZanimanjeId", "Naziv", uslugaLjudi.ZanimanjeId);
            return View(uslugaLjudi);
        }

        // POST: UslugaLjudi/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UslugaLjudiId,ZanimanjeId,UslugaId")] UslugaLjudi uslugaLjudi)
        {
            if (id != uslugaLjudi.UslugaLjudiId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uslugaLjudi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UslugaLjudiExists(uslugaLjudi.UslugaLjudiId))
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
            ViewData["UslugaId"] = new SelectList(_context.Usluga, "UslugaId", "Naziv", uslugaLjudi.UslugaId);
            ViewData["ZanimanjeId"] = new SelectList(_context.Zanimanje, "ZanimanjeId", "Naziv", uslugaLjudi.ZanimanjeId);
            return View(uslugaLjudi);
        }

        // GET: UslugaLjudi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uslugaLjudi = await _context.UslugaLjudi
                .Include(u => u.Usluga)
                .Include(u => u.Zanimanje)
                .FirstOrDefaultAsync(m => m.UslugaLjudiId == id);
            if (uslugaLjudi == null)
            {
                return NotFound();
            }

            return View(uslugaLjudi);
        }

        // POST: UslugaLjudi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uslugaLjudi = await _context.UslugaLjudi.FindAsync(id);
            _context.UslugaLjudi.Remove(uslugaLjudi);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UslugaLjudiExists(int id)
        {
            return _context.UslugaLjudi.Any(e => e.UslugaLjudiId == id);
        }
    }
}
