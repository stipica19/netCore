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
    public class UslugaOpremaController : Controller
    {
        private readonly PI01Context _context;

        public UslugaOpremaController(PI01Context context)
        {
            _context = context;
        }

        // GET: UslugaOprema
        public async Task<IActionResult> Index()
        {
            var pI01Context = _context.UslugaOprema.Include(u => u.ReferentniTipOpreme).Include(u => u.Usluga);
            return View(await pI01Context.ToListAsync());
        }

        // GET: UslugaOprema/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uslugaOprema = await _context.UslugaOprema
                .Include(u => u.ReferentniTipOpreme)
                .Include(u => u.Usluga)
                .FirstOrDefaultAsync(m => m.UslugaOpremaId == id);
            if (uslugaOprema == null)
            {
                return NotFound();
            }

            return View(uslugaOprema);
        }

        // GET: UslugaLjudi/Create
        public IActionResult Create(int uslugaId, int referentniTipOpremeId)
        {
           
            if (ModelState.IsValid)
            {
                
                try
                {
                    UslugaOprema uslugaOprema = new UslugaOprema();
                    uslugaOprema.UslugaId = uslugaId;
                    uslugaOprema.ReferentniTipOpremeId = referentniTipOpremeId;


                    _context.UslugaOprema.Add(uslugaOprema);
                    _context.SaveChanges();
                    //logger.LogInformation($"Osoba {osoba.Ime} dodana.");
                    // TempData[Constants.Message] = $"Osoba {} dodana.";
                    //  TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction("Index", "Usluga");

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
        // GET: UslugaOprema/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uslugaOprema = await _context.UslugaOprema.FindAsync(id);
            if (uslugaOprema == null)
            {
                return NotFound();
            }
            ViewData["ReferentniTipOpremeId"] = new SelectList(_context.ReferentniTipOpreme, "ReferentniTipOpremeId", "Naziv", uslugaOprema.ReferentniTipOpremeId);
            ViewData["UslugaId"] = new SelectList(_context.Usluga, "UslugaId", "Naziv", uslugaOprema.UslugaId);
            return View(uslugaOprema);
        }

        // POST: UslugaOprema/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UslugaOpremaId,UslugaId,ReferentniTipOpremeId")] UslugaOprema uslugaOprema)
        {
            if (id != uslugaOprema.UslugaOpremaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uslugaOprema);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UslugaOpremaExists(uslugaOprema.UslugaOpremaId))
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
            ViewData["ReferentniTipOpremeId"] = new SelectList(_context.ReferentniTipOpreme, "ReferentniTipOpremeId", "Naziv", uslugaOprema.ReferentniTipOpremeId);
            ViewData["UslugaId"] = new SelectList(_context.Usluga, "UslugaId", "Naziv", uslugaOprema.UslugaId);
            return View(uslugaOprema);
        }

        // GET: UslugaOprema/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uslugaOprema = await _context.UslugaOprema
                .Include(u => u.ReferentniTipOpreme)
                .Include(u => u.Usluga)
                .FirstOrDefaultAsync(m => m.UslugaOpremaId == id);
            if (uslugaOprema == null)
            {
                return NotFound();
            }

            return View(uslugaOprema);
        }

        // POST: UslugaOprema/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uslugaOprema = await _context.UslugaOprema.FindAsync(id);
            _context.UslugaOprema.Remove(uslugaOprema);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UslugaOpremaExists(int id)
        {
            return _context.UslugaOprema.Any(e => e.UslugaOpremaId == id);
        }
    }
}
