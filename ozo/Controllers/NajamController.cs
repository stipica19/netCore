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
    public class NajamController : Controller
    {
        private readonly PI01Context _context;
        private readonly AppSettings appData;
        private readonly ILogger logger;

        public NajamController(PI01Context context, IOptions<AppSettings> options, ILogger<NajamController> logger)
        {
            _context = context;
            appData = options.Value;
            this.logger = logger;
        }

        // GET: Oprema
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appData.PageSize;

            var query = _context.Vw_Najam
                           // .Include(c=>c.Registar)
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

            System.Linq.Expressions.Expression<Func<ViewNajam, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Kolicina;
                    break;
                case 2:
                    orderSelector = d => d.Cijena;
                    break;
                case 3:
                    orderSelector = d => d.Opis;
                    break;
                case 4:
                    orderSelector = d => d.DatumOd;
                    break;
                case 5:
                    orderSelector = d => d.DatumDo;
                    break;
                case 6:
                    orderSelector = d => d.NazivFirme;
                    break;
                case 7:
                    orderSelector = d => d.NazivOpreme;
                    break;
                case 8:
                    orderSelector = d => d.VrstaNajma;
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
            var model = new NajmoviViewModel
            {
                Najam = oprema,
                PagingInfo = pagingInfo
            };

            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            PrepareDropDownLists();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ViewNajam viewNajam)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Najam najam = new Najam();
                    najam.DatumDo = viewNajam.DatumDo;
                    najam.DatumOd = viewNajam.DatumOd;
                    najam.FimraId = viewNajam.FimraId;
                    najam.VrstaNajmaId = viewNajam.VrstaNajmaId;
                    najam.Opis = viewNajam.Opis;

                    Console.WriteLine("idees" + viewNajam.NajamId);

                    _context.Najam.Add(najam);

                    OpremaStavka oprStavka = new OpremaStavka();
                    oprStavka.NajamId = najam.NajamId;
                    oprStavka.OpremaId = viewNajam.OpremaId;
                    oprStavka.Kolicina = viewNajam.Kolicina;
                    oprStavka.Cijena = viewNajam.Cijena;

                    _context.OpremaStavka.Add(oprStavka);

                    _context.SaveChanges();
                    logger.LogInformation($"oprema {viewNajam.NajamId} dodan.");
                    TempData[Constants.Message] = $"Oprema {viewNajam.NajamId} dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception exc)
                {
                    logger.LogError("Pogreška prilikom dodavanje nove opreme: {0}", exc.CompleteExceptionMessage());
                    ModelState.AddModelError(string.Empty, errorMessage: exc.CompleteExceptionMessage());
                    return View(viewNajam);
                }
            }
            else
            {
                PrepareDropDownLists();
                return View(viewNajam);
            }

        }

        // GET: Najam/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var najam = await _context.Najam
                .Include(n => n.Firma)
                .Include(n => n.VrstaNajma)
                .FirstOrDefaultAsync(m => m.NajamId == id);
            if (najam == null)
            {
                return NotFound();
            }

            return View(najam);
        }

        // GET: Usluga/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Vw_Najam
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.NajamId == id);
            if (course == null)
            {
                return NotFound();
            }
            PrepareDropDownLists();
            return View(course);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(ViewNajam viewNajam)
        {

            var courseToUpdate = await _context.Najam
                                .FirstOrDefaultAsync(c => c.NajamId == viewNajam.NajamId);

            
            courseToUpdate.Opis = viewNajam.Opis;
            courseToUpdate.VrstaNajmaId = viewNajam.VrstaNajmaId;
            courseToUpdate.FimraId = viewNajam.FimraId;
            courseToUpdate.DatumDo = viewNajam.DatumDo;
            courseToUpdate.DatumOd = viewNajam.DatumOd;


            try
            {
                var courseToUpdateR = await _context.OpremaStavka
           .FirstOrDefaultAsync(c => c.NajamId == viewNajam.NajamId);
                courseToUpdateR.Kolicina = viewNajam.Kolicina;
                courseToUpdateR.Cijena = viewNajam.Cijena;
                courseToUpdateR.OpremaId = viewNajam.OpremaId;





                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException /* ex */)
            {
                ModelState.AddModelError("", "Neuspješno ažuriranje! ");
            }
           
            
            return RedirectToAction("Index", "Najam");

        }
        /// <summary>
        /// briše zapis iz baze
        /// </summary>
        /// <param name="id" name="?uslugaId">primarni kljucevi tabliceusluga</param>
        /// <returns>Glavni pogled</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int NajamId)
        {
            var najam = _context.Najam.Find(NajamId);
            if (najam != null)
            {
                try
                {

                    _context.Remove(_context.OpremaStavka.Single(a => a.NajamId == NajamId));
                   


                    int naziv = najam.NajamId;
                    _context.Remove(najam);
                    _context.SaveChanges();
                    logger.LogInformation($"najam {NajamId} uspješno obrisan.");
                    TempData[Constants.Message] = "Uspjesno obrisan najam: " + NajamId;
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    logger.LogError("Pogreška prilikom brisanja najma: " + exc.CompleteExceptionMessage());
                    TempData[Constants.Message] = "Pogreška prilikom brisanja najma: ";
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = "Ne postoji najam s oznakom: " + NajamId;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index));
        }

        

        private bool NajamExists(int id)
        {
            return _context.Najam.Any(e => e.NajamId == id);
        }
        private void PrepareDropDownLists()
        {
          
            var OpremaList = _context.Oprema
                       .AsNoTracking()
                         .FromSql("SELECT * FROM dbo.Oprema")
                        
                        .ToList();
            ViewBag.OpremaList = new SelectList(OpremaList, nameof(Oprema.OpremaId), nameof(Oprema.Naziv));

            var FirmaList = _context.Registar
                      .AsNoTracking()
                       .FromSql("SELECT * FROM dbo.Registar")
                       .Where(v=>v.TipRegistraId==2)

                       .ToList();
            ViewBag.FirmaList = new SelectList(FirmaList, nameof(Registar.RegistarId), nameof(TipRegistra.Naziv));

            var NajamList = _context.Registar
                     .AsNoTracking()
                      .FromSql("SELECT * FROM dbo.Registar")
                      .Where(v => v.TipRegistraId ==8)

                      .ToList();
            ViewBag.NajamList = new SelectList(NajamList, nameof(Registar.RegistarId), nameof(TipRegistra.Naziv));


        }
    }
}
