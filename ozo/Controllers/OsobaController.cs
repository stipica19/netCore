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
    public class OsobaController : Controller
    {
        private readonly PI01Context _context;
        private readonly AppSettings appData;
        private readonly ILogger logger;

        public OsobaController(PI01Context context, IOptions<AppSettings> options, ILogger<OsobaController> logger)
        {
            this._context = context;
            appData = options.Value;
            this.logger = logger;
        }

        // GET: Osoba
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {

            int pagesize = appData.PageSize;

            var query = _context.Vw_Osoba

                        .AsNoTracking();

            int count = query.Count();
            if (count == 0)
            {
                TempData[Constants.Message] = "Ne postoji niti jedana osoba.";
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

            System.Linq.Expressions.Expression<Func<ViewOsoba, object>> orderSelector = null;

            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Ime;
                    break;
                case 2:
                    orderSelector = d => d.Prezime;
                    break;
                case 3:
                    orderSelector = d => d.God_rodjenja;
                    break;
                case 4:
                    orderSelector = d => d.NazivCertifikata;
                    break;
                case 5:
                    orderSelector = d => d.NazivZanimanja;
                    break;

            }
            if (orderSelector != null)
            {
                query = ascending ?
                       query.OrderBy(orderSelector) :
                       query.OrderByDescending(orderSelector);
            }

            var herbar = query
                        .Skip((page - 1) * pagesize)
                        .FromSql("Select * FROM dbo.Vw_Osoba")
                        .Take(pagesize)
                        .ToList();
            var model = new OsobeViewModel
            {
                Osoba = herbar,
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
        public IActionResult Create(ViewOsoba osobaView)
        {
            if (ModelState.IsValid)
            {


                try
                {
                    Osoba osoba = new Osoba();
                    osoba.Ime = osobaView.Ime;
                    osoba.Prezime = osobaView.Prezime;
                    osoba.GodRodjenja = osobaView.God_rodjenja;

                    _context.Osoba.Add(osoba);
  
                    OsobaCertifikat osobaCert = new OsobaCertifikat();
                    osobaCert.CertifikatId = osobaView.CertifikatId;
                    osobaCert.OsobaId = osobaView.OsobaId;
                   
   

                    Radnik radnik = new Radnik();
                    radnik.OsobaId = osobaView.OsobaId;
                    radnik.KategorijaId = osobaView.ZanimanjeId;
                 
                    _context.SaveChanges();

                    logger.LogInformation($"Osoba {osoba.Ime} dodana.");
                    TempData[Constants.Message] = $"Osoba {osoba.Ime} dodana.";
                    TempData[Constants.ErrorOccurred] = false;

                    return RedirectToAction("Create", "OsobaCertifikat", new { osoba.OsobaId, osobaView.CertifikatId, osobaView.ZanimanjeId });
                    //return RedirectToAction("Index", "Osoba");
                }
                catch (Exception exc)
                {
                     logger.LogError("Pogreška prilikom dodavanje nove osobe: {0}", exc.CompleteExceptionMessage());
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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int OsobaId)
        {
            var osoba = _context.Osoba.Find(OsobaId);
            if (osoba != null)
            {
                try
                {

                    _context.Remove(_context.OsobaCertifikat.Single(a => a.OsobaId == OsobaId));
                    _context.Remove(_context.Radnik.Single(a => a.OsobaId == OsobaId));


                    int naziv = osoba.OsobaId;
                    _context.Remove(osoba);
                    _context.SaveChanges();
                    logger.LogInformation($"oprema {naziv} uspješno obrisan.");
                    TempData[Constants.Message] = "Uspjesno obrisana osoba: " + osoba.Ime;
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception)
                {
                    //logger.LogError("Pogreška prilikom brisanja opreme: " + exc.CompleteExceptionMessage());
                    TempData[Constants.Message] = "Pogreška prilikom brisanja osobe: ";
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = "Ne postoji osoba s oznakom: " + OsobaId;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index));
        }



        // GET: Osoba/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Vw_Osoba
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.OsobaId == id);
            if (course == null)
            {
                return NotFound();
            }
            PrepareDropDownLists();
            return View(course);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(ViewOsoba osobaView)
        {
            Console.WriteLine("dinamo" + osobaView.CertifikatId);


            var courseToUpdate = await _context.Osoba
                .Include(c=>c.OsobaCertifikat)
                .FirstOrDefaultAsync(c => c.OsobaId == osobaView.OsobaId);

            courseToUpdate.Ime = osobaView.Ime;
            courseToUpdate.Prezime = osobaView.Prezime;
            courseToUpdate.GodRodjenja = osobaView.God_rodjenja;

            try
                {
                    var courseToUpdateR = await _context.Radnik
               .FirstOrDefaultAsync(c => c.OsobaId == osobaView.OsobaId);
                    courseToUpdateR.KategorijaId = osobaView.ZanimanjeId;
                    var courseToUpdateC = await _context.OsobaCertifikat
               .FirstOrDefaultAsync(c => c.OsobaId == osobaView.OsobaId);
                    courseToUpdateC.CertifikatId = osobaView.CertifikatId;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Neuspješno ažuriranje! ");
                }
                Console.WriteLine("Cola" + osobaView.ZanimanjeId);


                // return RedirectToAction("EditPost", "OsobaCertifikat", new { osobaView.OsobaId, osobaView.CertifikatId, osobaView.ZanimanjeId});
                return RedirectToAction("Index", "Osoba");
            
        }













        private bool OsobaExists(int id)
        {
            return _context.Osoba.Any(e => e.OsobaId == id);
        }

        private void PrepareDropDownLists()
        {

            var StatusOpreme = _context.Registar
                .AsNoTracking()
                        .FromSql("SELECT * FROM dbo.Registar")
                        .Where(c => c.TipRegistraId == 4)

                        .ToList();
            ViewBag.StatusOpreme = new SelectList(StatusOpreme, nameof(Registar.RegistarId), nameof(TipRegistra.Naziv));

            var ZanimanjeList = _context.Zanimanje
                .AsNoTracking()
                        .FromSql("SELECT * FROM dbo.Zanimanje")


                        .ToList();
            ViewBag.ZanimanjeList = new SelectList(ZanimanjeList, nameof(Zanimanje.ZanimanjeId), nameof(Zanimanje.Naziv));


        }
    }
}