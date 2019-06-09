using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ozo.Extensions;
using ozo.Models;
using ozo.ViewModels;

namespace ozo.Controllers
{
    public class NatjecajController : Controller
    {
        private readonly PI01Context _context;
        private readonly AppSettings appData;
        private readonly ILogger logger;

        public NatjecajController(PI01Context context, IOptions<AppSettings> options, ILogger<NatjecajController> logger)
        {
            _context = context;
            appData = options.Value;
            this.logger = logger;
        }

        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appData.PageSize;

            var query = _context.Natjecaj
                             .Include(c => c.VrstaNatjecaja)
                             .Include(d => d.Raspisatelj)
                             .Include(e => e.Pobiednik)
                             .AsNoTracking();

            int count = query.Count();
            if (count == 0)
            {
                TempData[Constants.Message] = "Ne postoji niti jedan servis.";
                TempData[Constants.ErrorOccurred] = false;
                return RedirectToAction(nameof(Create));
            }

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                Sort = sort,
                Ascending = ascending,
                ItemsPerPage = pagesize,
                TotalItems = count
            };
            if (page > pagingInfo.TotalPages)
            {
                return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort = sort, ascending = ascending });
            }

            System.Linq.Expressions.Expression<Func<Natjecaj, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.NatjecajId;
                    break;
               

            }
            if (orderSelector != null)
            {
                query = ascending ?
                       query.OrderBy(orderSelector) :
                       query.OrderByDescending(orderSelector);
            }
            var natjecaj = query
                        .Skip((page - 1) * pagesize)
                        .Take(pagesize)
                        .ToList();
            var model = new ViewNatjecajModel
            {
                Natjecaj = natjecaj,
                PagingInfo = pagingInfo
            };

            return View(model);
        }


        // GET: Natjecaj/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var natjecaj = await _context.Natjecaj
                .Include(n => n.Pobiednik)
                .Include(n => n.Raspisatelj)
                .Include(n => n.VrstaNatjecaja)
                .FirstOrDefaultAsync(m => m.NatjecajId == id);
            if (natjecaj == null)
            {
                return NotFound();
            }

            return View(natjecaj);
        }

        // GET: Natjecaj/Create
        [HttpGet]
        public IActionResult Create()
        {
            PrepareDropDownLists();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Natjecaj natjecaj)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    _context.Add(natjecaj);
                    _context.SaveChanges();
                    //return RedirectToAction("Index", "Usluga");
                     logger.LogInformation($"Natjecaj {natjecaj.NatjecajId} dodan.");
                    TempData[Constants.Message] = $"Natjecaj {natjecaj.NatjecajId} dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception exc)
                {
                    logger.LogError("Pogreška prilikom dodavanje novog natjecaja: {0}", exc.CompleteExceptionMessage());
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    PrepareDropDownLists();
                    return View(natjecaj);
                }
            }
            else
            {
                PrepareDropDownLists();
                return View(natjecaj);
            }

        }

        // GET: Natjecaj/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var natjecaj = await _context.Natjecaj.FindAsync(id);
            if (natjecaj == null)
            {
                return NotFound();
            }
            ViewData["PobiednikId"] = new SelectList(_context.Registar, "RegistarId", "Naziv", natjecaj.PobiednikId);
            ViewData["RaspisateljId"] = new SelectList(_context.Registar, "RegistarId", "Naziv", natjecaj.RaspisateljId);
            ViewData["VrstaNatjecajaId"] = new SelectList(_context.Registar, "RegistarId", "Naziv", natjecaj.VrstaNatjecajaId);
            return View(natjecaj);
        }

        // POST: Natjecaj/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NatjecajId,Naziv,Opis,Vrijednost,VrijemeOd,VrijemeDo,VrstaNatjecajaId,RaspisateljId,PobiednikId")] Natjecaj natjecaj)
        {
            if (id != natjecaj.NatjecajId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(natjecaj);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NatjecajExists(natjecaj.NatjecajId))
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
            ViewData["PobiednikId"] = new SelectList(_context.Registar, "RegistarId", "Naziv", natjecaj.PobiednikId);
            ViewData["RaspisateljId"] = new SelectList(_context.Registar, "RegistarId", "Naziv", natjecaj.RaspisateljId);
            ViewData["VrstaNatjecajaId"] = new SelectList(_context.Registar, "RegistarId", "Naziv", natjecaj.VrstaNatjecajaId);
            return View(natjecaj);
        }

        // GET: Natjecaj/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var natjecaj = await _context.Natjecaj
                .Include(n => n.Pobiednik)
                .Include(n => n.Raspisatelj)
                .Include(n => n.VrstaNatjecaja)
                .FirstOrDefaultAsync(m => m.NatjecajId == id);
            if (natjecaj == null)
            {
                return NotFound();
            }

            return View(natjecaj);
        }

        // POST: Natjecaj/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var natjecaj = await _context.Natjecaj.FindAsync(id);
            _context.Natjecaj.Remove(natjecaj);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NatjecajExists(int id)
        {
            return _context.Natjecaj.Any(e => e.NatjecajId == id);
        }

        private void PrepareDropDownLists()
        {


            var Raspisatelj = _context.Registar
                         .AsNoTracking()
                         .FromSql("SELECT * FROM dbo.Registar")
                        .Where(c => c.TipRegistraId == 2)
                        .ToList();
            ViewBag.Raspisatelj = new SelectList(Raspisatelj, nameof(Registar.RegistarId), nameof(TipRegistra.Naziv));

            //var Pobjednik = _context.Registar
            //           .AsNoTracking()
            //           .FromSql("SELECT * FROM dbo.Registar")
            //          .Where(c => c.TipRegistraId == 2)
            //          .ToList();
            //ViewBag.Pobjednik = new SelectList(Pobjednik, nameof(Registar.RegistarId), nameof(TipRegistra.Naziv));

            var vrsta = _context.Registar
                       .AsNoTracking()
                       .FromSql("SELECT * FROM dbo.Registar")
                      .Where(c => c.TipRegistraId == 7)
                      .ToList();
            ViewBag.vrsta = new SelectList(vrsta, nameof(Registar.RegistarId), nameof(TipRegistra.Naziv));
        }
    }
}
