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
    public class UslugaController : Controller
    {
        private readonly PI01Context _context;
        private readonly AppSettings appData;
        private readonly ILogger logger;

        public UslugaController(PI01Context context, IOptions<AppSettings> options, ILogger<OsobaController> logger)
        {
            _context = context;
            this.logger = logger;
            appData = options.Value;
        }

        // GET: Usluga
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {

            int pagesize = appData.PageSize;

            var query = _context.Vw_Usluga

                        .AsNoTracking();

            int count = query.Count();
            if (count == 0)
            {
                TempData[Constants.Message] = "Ne postoji niti jedana usluga.";
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

            System.Linq.Expressions.Expression<Func<ViewUsluga, object>> orderSelector = null;

            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Naziv;
                    break;
                case 2:
                    orderSelector = d => d.Opis;
                    break;
                case 3:
                    orderSelector = d => d.KategorijaPoslaNaziv;
                    break;
                case 4:
                    orderSelector = d => d.NazivTipaOpreme;
                    break;
                case 5:
                    orderSelector = d => d.NazivTipaZanimanja;
                    break;



            }
            if (orderSelector != null)
            {
                query = ascending ?
                       query.OrderBy(orderSelector) :
                       query.OrderByDescending(orderSelector);
            }

            var uluga = query
                        .Skip((page - 1) * pagesize)
                        .FromSql("Select * FROM dbo.Vw_Usluga")
                        .Take(pagesize)
                        .ToList();
            var model = new UslugeViewModel
            {
                Usluga = uluga,
                PagingInfo = pagingInfo
            };

            return View(model);

        }

        // GET: Usluga/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usluga = await _context.Usluga
                .Include(u => u.KategorijaPosla)
                .FirstOrDefaultAsync(m => m.UslugaId == id);
            if (usluga == null)
            {
                return NotFound();
            }

            return View(usluga);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PrepareDropDownLists();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ViewUsluga uslugaView)
        {
            if (ModelState.IsValid)
            {
                try
                {


                    Usluga usluga = new Usluga();
                    usluga.Naziv = uslugaView.Naziv;
                    usluga.Opis = uslugaView.Opis;
                    usluga.KategorijaPoslaId = uslugaView.KategorijaPoslaId;


                    _context.Add(usluga);
                    _context.SaveChanges();
                    logger.LogInformation($"Usluga {usluga.UslugaId} dodan.");
                    TempData[Constants.Message] = $"Usluga {usluga.Naziv} dodana.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction("Create", "UslugaLjudi", new { usluga.UslugaId, uslugaView.ZanimanjeId, uslugaView.ReferentniTipOpremeId});


                }
                catch (Exception exc)
                {
                    logger.LogError("Pogreška prilikom dodavanje nove usluge: {0}", exc.CompleteExceptionMessage());
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View();
                }
            }
            else
            {
                PrepareDropDownLists();
                return View();
            }

        }



        // GET: Usluga/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Vw_Usluga
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.UslugaId == id);
            if (course == null)
            {
                return NotFound();
            }
            PrepareDropDownLists();
            return View(course);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(ViewUsluga uslugaView)
        {
        
            var courseToUpdate = await _context.Usluga
                                .FirstOrDefaultAsync(c => c.UslugaId == uslugaView.UslugaId);

            courseToUpdate.Naziv = uslugaView.Naziv;
            courseToUpdate.Opis = uslugaView.Opis;
            courseToUpdate.KategorijaPoslaId = uslugaView.KategorijaPoslaId;

            try
            {
                var courseToUpdateR = await _context.UslugaLjudi
           .FirstOrDefaultAsync(c => c.UslugaId == uslugaView.UslugaId);
                courseToUpdateR.ZanimanjeId = uslugaView.ZanimanjeId;

                var courseToUpdateC = await _context.UslugaOprema
           .FirstOrDefaultAsync(c => c.UslugaId == uslugaView.UslugaId);
                courseToUpdateC.ReferentniTipOpremeId = uslugaView.ReferentniTipOpremeId;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException /* ex */)
            {
                ModelState.AddModelError("", "Neuspješno ažuriranje! ");
            }
            Console.WriteLine("COla" + uslugaView.ZanimanjeId);


            // return RedirectToAction("EditPost", "OsobaCertifikat", new { osobaView.OsobaId, osobaView.CertifikatId, osobaView.ZanimanjeId});
            return RedirectToAction("Index", "Usluga");

        }
        /// <summary>
        /// briše zapis iz baze
        /// </summary>
        /// <param name="id" name="?uslugaId">primarni kljucevi tabliceusluga</param>
        /// <returns>Glavni pogled</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int UslugaId)
        {
            var usluga = _context.Usluga.Find(UslugaId);
            if (usluga != null)
            {
                try
                {

                    _context.Remove(_context.UslugaLjudi.Single(a => a.UslugaId == UslugaId));
                    _context.Remove(_context.UslugaOprema.Single(a => a.UslugaId == UslugaId));


                    int naziv = usluga.UslugaId;
                    _context.Remove(usluga);
                    _context.SaveChanges();
                    logger.LogInformation($"Usluga {naziv} uspješno obrisan.");
                    TempData[Constants.Message] = "Uspjesno obrisana usluga: " + usluga.Naziv;
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    logger.LogError("Pogreška prilikom brisanja opreme: " + exc.CompleteExceptionMessage());
                    TempData[Constants.Message] = "Pogreška prilikom brisanja usluge: ";
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = "Ne postoji usluga s oznakom: " + UslugaId;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index));
        }




        private bool UslugaExists(int id)
        {
            return _context.Usluga.Any(e => e.UslugaId == id);
        }

        private void PrepareDropDownLists()
        {
            var TipZanimanja = _context.Zanimanje
                       .AsNoTracking()
                       
                       .ToList();
            ViewBag.TipZanimanja = new SelectList(TipZanimanja, nameof(Zanimanje.ZanimanjeId), nameof(Zanimanje.Naziv));


            var katPosla = _context.KategorijaPosla
                        .AsNoTracking()
                        .ToList();
            ViewBag.katPosla = new SelectList(katPosla, nameof(KategorijaPosla.KategorijaPoslaId), nameof(KategorijaPosla.Naziv));



            var TipOpreme = _context.ReferentniTipOpreme
                        .AsNoTracking()
                        .ToList();
            ViewBag.TipOpreme = new SelectList(TipOpreme, nameof(ReferentniTipOpreme.ReferentniTipOpremeId), nameof(ReferentniTipOpreme.Naziv));

          


        }
    }
}
