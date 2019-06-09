using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ozo.Models;
using ozo.ViewModels;


namespace ozo.Controllers
{
    public class PosaoController : Controller
    {
        private readonly PI01Context _context;
        private readonly AppSettings appData;
        private readonly ILogger logger;

        public PosaoController(PI01Context context, IOptions<AppSettings> options, ILogger<PosaoController> logger)
        {
            _context = context;
            appData = options.Value;
            this.logger = logger;
        }


        // GET: Posao
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {

            int pagesize = appData.PageSize;

            var query = _context.Vw_PO
                 
                        .AsNoTracking();

            int count = query.Count();
            if (count == 0)
            {
                TempData[Constants.Message] = "Ne postoji niti jedan Herbar.";
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

            System.Linq.Expressions.Expression<Func<ViewPosao, object>> orderSelector = null;
        
            var herbar = query
                        .Skip((page - 1) * pagesize)
                        .FromSql("Select * FROM dbo.Vw_PO")
                        .Take(pagesize)
                        .ToList();
            var model = new PosaoViewModel
            {
                Poslovi = herbar,
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
        public IActionResult Create(Posao posao)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(posao);
                    _context.SaveChanges();
                    logger.LogInformation($"Osoba {posao.PosaoId} dodana.");
                    TempData[Constants.Message] = $"Posao {posao.PosaoId} dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception)
                {
                    //  logger.LogError("Pogreška prilikom dodavanje nove osobe: {0}", exc.CompleteExceptionMessage());
                    //  ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(posao);
                }
            }
            else
            {
                PrepareDropDownLists();
                return View(posao);
            }

        }

        // GET: Posao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Posao
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.PosaoId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseToUpdate = await _context.Posao
                .FirstOrDefaultAsync(c => c.PosaoId == id);

            if (await TryUpdateModelAsync<Posao>(courseToUpdate,
                "",
                c => c.Opis, c => c.Cijena, c => c.DodatniTrosak, c => c.VrijemeOd, c => c.VrijemeDo, c => c.UslugaId, c => c.LokacijaPosla, c => c.NatjecajId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Neuspješno ažuriranje! ");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(courseToUpdate);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int PosaoId)
        {
            var osoba = _context.Posao.Find(PosaoId);
            if (osoba != null)
            {
                try
                {
                    int naziv = osoba.PosaoId;
                    _context.Remove(osoba);
                    _context.SaveChanges();
                    //logger.LogInformation($"oprema {naziv} uspješno obrisan.");

                    //TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception)
                {
                    //logger.LogError("Pogreška prilikom brisanja opreme: " + exc.CompleteExceptionMessage());
                    TempData[Constants.Message] = "Pogreška prilikom brisanja opreme: ";
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = "Ne postoji oprema s oznakom: " + PosaoId;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index));
        }

        

        private bool PosaoExists(int id)
        {
            return _context.Posao.Any(e => e.PosaoId == id);
        }


        








        private void PrepareDropDownLists()
        {
            

            var UslugaList = _context.Usluga
                        .AsNoTracking()
                        .ToList();
            ViewBag.UslugaList = new SelectList(UslugaList, nameof(Usluga.UslugaId), nameof(Usluga.Naziv));

            var lokacija = _context.LokacijaPosla
                        .AsNoTracking()
                        .ToList();
            ViewBag.LokacijaList = new SelectList(lokacija, nameof(LokacijaPosla.LokacijaPoslaId), nameof(LokacijaPosla.NazivLokacije));

            var NatjecajList = _context.Natjecaj
                        .AsNoTracking()
                     
                        .ToList();
            ViewBag.NatjecajList = new SelectList(NatjecajList, nameof(Natjecaj.NatjecajId), nameof(Natjecaj.Naziv));


        }

    }
}
