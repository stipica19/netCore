using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebSockets.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ozo.Extensions;
using ozo.Models;
using ozo.ViewModels;



namespace ozo.Controllers
{
    public class OpremaController : Controller
    {
        private readonly PI01Context _context;
        private readonly AppSettings appData;
        private readonly ILogger logger;

        public OpremaController(PI01Context context, IOptions<AppSettings> options, ILogger<OpremaController> logger)
        {
            _context = context;
            appData = options.Value;
            this.logger = logger;
        }

        // GET: Oprema
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appData.PageSize;

            var query = _context.Oprema
                             .Include(c => c.Status)
                             .Include(d => d.ReferentniTipOpreme)
                             .Include(e => e.LokacijaOpreme)
                             .AsNoTracking();

            int count = query.Count();
            if (count == 0)
            {
                TempData[Constants.Message] = "Ne postoji niti jedana Oprema.";
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

            System.Linq.Expressions.Expression<Func<Oprema, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.InventarniBroj;
                    break;
                case 2:
                    orderSelector = d => d.Naziv;
                    break;
                case 3:
                    orderSelector = d => d.KnjigovostvenaVrijednost;
                    break;
                case 4:
                    orderSelector = d => d.LokacijaOpreme;
                    break;
                case 5:
                    orderSelector = d => d.ReferentniTipOpreme;
                    break;
                case 6:
                    orderSelector = d => d.Status;
                    break;
            }
            if (orderSelector != null)
            {
                query = ascending ?
                       query.OrderBy(orderSelector) :
                       query.OrderByDescending(orderSelector);
            }
            var oprema = query
                        .Skip((page - 1) * pagesize)
                        .Take(pagesize)
                        .ToList();
            var model = new OpremaModel
            {
                Oprema = oprema,
                PagingInfo = pagingInfo
            };

            return View(model);
        }

        // GET: Oprema/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oprema = await _context.Oprema
                .Include(o => o.LokacijaOpreme)
                .Include(o => o.ReferentniTipOpreme)
                .Include(o => o.Status)
                .FirstOrDefaultAsync(m => m.OpremaId == id);
            if (oprema == null)
            {
                return NotFound();
            }

            return View(oprema);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PrepareDropDownLists();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Oprema oprema)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(oprema);
                    _context.SaveChanges();
                    logger.LogInformation($"oprema {oprema.Naziv} dodan.");
                    TempData[Constants.Message] = $"Oprema {oprema.Naziv} dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception exc)
                {
                   logger.LogError("Pogreška prilikom dodavanje nove opreme: {0}", exc.CompleteExceptionMessage());
                   ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(oprema);
                }
            }
            else
            {
                PrepareDropDownLists();
                return View(oprema);
            }

        }

        // GET: Oprema/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var oprema = _context.Oprema.Find(id);
            if (oprema == null)
            {
                logger.LogWarning("Ne postoji oprema s oznakom: {0} ", id);
                return NotFound("Ne postoji oprema s oznakom: " + id);
            }
            else
            {
                PrepareDropDownLists();
                return View(oprema);
            }
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oprema = await _context.Oprema
                .FirstOrDefaultAsync(c => c.OpremaId == id);

            if (await TryUpdateModelAsync<Oprema>(oprema,
                "",
                c => c.InventarniBroj,
                c => c.Naziv,
                c => c.KnjigovostvenaVrijednost,
                c => c.ReferentniTipOpremeId,
                c => c.LokacijaOpremeId,
                c => c.StatusId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    TempData[Constants.Message] = "Uspjesno azuriranje opreme "+ oprema.Naziv;
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Neuspješno ažuriranje! ");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(oprema);
        }

        // GET: Oprema/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int OpremaId)
        {
            var oprema = _context.Oprema.Find(OpremaId);
            if (oprema != null)
            {
                try
                {
                    int naziv = oprema.OpremaId;
                    _context.Remove(oprema);
                    _context.SaveChanges();
                    logger.LogInformation($"oprema {naziv} uspješno obrisan.");

                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                     logger.LogError("Pogreška prilikom brisanja opreme: " + exc.CompleteExceptionMessage());
                    logger.LogError("Pogreška prilikom brisanja opreme: " + exc.CompleteExceptionMessage());
                    TempData[Constants.Message] = "Pogreška prilikom brisanja opreme: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = "Ne postoji oprema s oznakom: " + OpremaId;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index));
        }


        private bool OpremaExists(int id)
        {
            return _context.Oprema.Any(e => e.OpremaId == id);
        }



        private void PrepareDropDownLists()
        {
            var referentniTipOpreme = _context.ReferentniTipOpreme
                        .AsNoTracking()
                        .ToList();
            ViewBag.ReferentniTipOpreme = new SelectList(referentniTipOpreme, nameof(ReferentniTipOpreme.ReferentniTipOpremeId), nameof(ReferentniTipOpreme.Naziv));

            var lokacijaOpreme = _context.LokacijaOpreme
                        .AsNoTracking()
                        .ToList();
            ViewBag.LokacijaOpreme = new SelectList(lokacijaOpreme, nameof(LokacijaOpreme.LokacijaOpremeId), nameof(LokacijaOpreme.NazivLokacije));

            var StatusOpreme = _context.Registar
                .AsNoTracking()
                        .FromSql("SELECT * FROM dbo.Registar")
                        .Where(c=>c.TipRegistraId==3)
                        //.Where(c => c.Naziv == "Nedostupno")

                        .ToList();
            ViewBag.StatusOpreme = new SelectList(StatusOpreme, nameof(Registar.RegistarId), nameof(TipRegistra.Naziv));


        }
    }
}
