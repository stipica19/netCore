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
    public class ServisController : Controller
    {
        private readonly PI01Context _context;
        private readonly AppSettings appData;
        private readonly ILogger logger;

        public ServisController(PI01Context context, IOptions<AppSettings> options, ILogger<OpremaController> logger)
        {
            _context = context;
            appData = options.Value;
            this.logger = logger;
        }

        // GET: Servis
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appData.PageSize;

            var query = _context.Servis
                             .Include(c => c.Oprema)
                             .Include(d => d.Serviser)
                             .Include(e => e.Osoba)
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

            System.Linq.Expressions.Expression<Func<Servis, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Serviser;
                    break;
                case 2:
                    orderSelector = d => d.Osoba;
                    break;
                case 3:
                    orderSelector = d => d.Oprema;
                    break;
                case 4:
                    orderSelector = d => d.Opis;
                    break;
                case 5:
                    orderSelector = d => d.Cijena;
                    break;

            }
            if (orderSelector != null)
            {
                query = ascending ?
                       query.OrderBy(orderSelector) :
                       query.OrderByDescending(orderSelector);
            }
            var servis = query
                        .Skip((page - 1) * pagesize)
                        .Take(pagesize)
                        .ToList();
            var model = new ViewServisModel
            {
                Servis = servis,
                PagingInfo = pagingInfo
            };

            return View(model);
        }

        // GET: Servis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servis = await _context.Servis
                .Include(s => s.Oprema)
                .Include(s => s.Osoba)
                .Include(s => s.Serviser)
                .FirstOrDefaultAsync(m => m.ServisId == id);
            if (servis == null)
            {
                return NotFound();
            }

            return View(servis);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PrepareDropDownLists();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Servis servis)
        {
            if (ModelState.IsValid)
            {
                
                try
                {
                    _context.Add(servis);
                    _context.SaveChanges();
                    //return RedirectToAction("Index", "Usluga");
                   logger.LogInformation($"Servis {servis.ServisId} dodan.");
                    TempData[Constants.Message] = $"Servis {servis.ServisId} dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception exc)
                {
                    logger.LogError("Pogreška prilikom dodavanje novog autora: {0}", exc.CompleteExceptionMessage());
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(servis);
                }
            }
            else
            {
                PrepareDropDownLists();
                return View(servis);
            }

        }

        // GET: Oprema/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var servis = _context.Servis.Find(id);
            if (servis == null)
            {
                logger.LogWarning("Ne postoji servis s oznakom: {0} ", id);
                return NotFound("Ne postoji servis s oznakom: " + id);
            }
            else
            {
                PrepareDropDownLists();
                return View(servis);
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

            var servis = await _context.Servis
                .FirstOrDefaultAsync(c => c.ServisId == id);

            if (await TryUpdateModelAsync<Servis>(servis,
                "",
                b=>b.Opis,
                b=>b.Cijena,
                b=>b.ServisId,
                b => b.ServiserId,
                b=>b.OpremaId,
                b=>b.OsobaId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    TempData[Constants.Message] = "Uspjesno azuriranje servisa " + servis.ServisId;
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Neuspješno ažuriranje! ");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(servis);
        }

        // GET: Oprema/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int ServisId)
        {
            var servis = _context.Servis.Find(ServisId);
            if (servis != null)
            {
                try
                {
                    int naziv = servis.ServisId;
                    _context.Remove(servis);
                    _context.SaveChanges();
                    logger.LogInformation($"Servis {naziv} uspješno obrisan.");

                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    logger.LogError("Pogreška prilikom brisanja servisa: " + exc.CompleteExceptionMessage());
                    logger.LogError("Pogreška prilikom brisanja servisa: ");
                    TempData[Constants.Message] = "Pogreška prilikom brisanja servisa: ";
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = "Ne postoji servis s oznakom: " + ServisId;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index));
        }
        private bool ServisExists(int id)
        {
            return _context.Servis.Any(e => e.ServisId == id);
        }

         private void PrepareDropDownLists(){

            var OsobaList = _context.Osoba
                        .AsNoTracking()
                        .ToList();
            ViewBag.OsobaList = new SelectList(OsobaList, nameof(Osoba.OsobaId), nameof(Osoba.Ime));

            var OpremaList = _context.Oprema
                        .AsNoTracking()
                        .ToList();
            ViewBag.OpremaList = new SelectList(OpremaList, nameof(Oprema.OpremaId), nameof(Oprema.Naziv));

            var ServiserList = _context.Registar
                .AsNoTracking()
                         .FromSql("SELECT * FROM dbo.Registar")
                        .Where(c => c.TipRegistraId == 5)

                        .ToList();
            ViewBag.ServiserList = new SelectList(ServiserList, nameof(Registar.RegistarId), nameof(TipRegistra.Naziv));
        }
        }
    }
