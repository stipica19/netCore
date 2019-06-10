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
    public class PosaoOpremaController : Controller
    {
        private readonly PI01Context _context;

        public PosaoOpremaController(PI01Context context)
        {
            _context = context;
        }

        // GET: PosaoOprema
        public async Task<IActionResult> Index()
        {
            var pI01Context = _context.PosaoOprema.Include(p => p.Oprema).Include(p => p.Posao);
            return View(await pI01Context.ToListAsync());
        }

        // GET: PosaoOprema/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posaoOprema = await _context.PosaoOprema
                .Include(p => p.Oprema)
                .Include(p => p.Posao)
                .FirstOrDefaultAsync(m => m.PosaoOpremaId == id);
            if (posaoOprema == null)
            {
                return NotFound();
            }

            return View(posaoOprema);
        }

        public IActionResult Create(int posaoId, int opremaId)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    PosaoOprema posaoOprema = new PosaoOprema();
                    posaoOprema.PosaoId = posaoId;
                    posaoOprema.OpremaId = opremaId;


                    _context.Add(posaoOprema);
                    _context.SaveChanges();
                   // logger.LogInformation($"Osoba {posaoOprema.PosaoId} dodana.");
                    //TempData[Constants.Message] = $"Osoba {} dodana.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction("Index", "Posao");

                }
                catch (Exception exc)
                {
                    //  logger.LogError("Pogreška prilikom dodavanje nove osobe: {0}", exc.CompleteExceptionMessage());
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
        // GET: PosaoOprema/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posaoOprema = await _context.PosaoOprema.FindAsync(id);
            if (posaoOprema == null)
            {
                return NotFound();
            }
            ViewData["OpremaId"] = new SelectList(_context.Oprema, "OpremaId", "Naziv", posaoOprema.OpremaId);
            ViewData["PosaoId"] = new SelectList(_context.Posao, "PosaoId", "Opis", posaoOprema.PosaoId);
            return View(posaoOprema);
        }

        // POST: PosaoOprema/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PosaoOpremaId,OpremaId,PosaoId")] PosaoOprema posaoOprema)
        {
            if (id != posaoOprema.PosaoOpremaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(posaoOprema);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PosaoOpremaExists(posaoOprema.PosaoOpremaId))
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
            ViewData["OpremaId"] = new SelectList(_context.Oprema, "OpremaId", "Naziv", posaoOprema.OpremaId);
            ViewData["PosaoId"] = new SelectList(_context.Posao, "PosaoId", "Opis", posaoOprema.PosaoId);
            return View(posaoOprema);
        }

        // GET: PosaoOprema/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posaoOprema = await _context.PosaoOprema
                .Include(p => p.Oprema)
                .Include(p => p.Posao)
                .FirstOrDefaultAsync(m => m.PosaoOpremaId == id);
            if (posaoOprema == null)
            {
                return NotFound();
            }

            return View(posaoOprema);
        }

        // POST: PosaoOprema/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var posaoOprema = await _context.PosaoOprema.FindAsync(id);
            _context.PosaoOprema.Remove(posaoOprema);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PosaoOpremaExists(int id)
        {
            return _context.PosaoOprema.Any(e => e.PosaoOpremaId == id);
        }
    }
}
