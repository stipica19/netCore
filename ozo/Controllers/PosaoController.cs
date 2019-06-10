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
    public class PosaoController : Controller
    {
        private readonly PI01Context _context;
        private readonly AppSettings appData;
        private readonly ILogger logger;

        public PosaoController(PI01Context context, IOptions<AppSettings> options, ILogger<PosaoController> logger)
        {
            _context = context;
            this.logger = logger;
            appData = options.Value;
        }

        // GET: Posoa
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {

            int pagesize = appData.PageSize;

            var query = _context.Vw_Posao

                        .AsNoTracking();

            int count = query.Count();
            if (count == 0)
            {
                TempData[Constants.Message] = "Ne postoji niti jedan posao.";
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

            switch (sort)
            {
                case 1:
                    orderSelector = d => d.PosaoId;
                    break;
               
            }
            if (orderSelector != null)
            {
                query = ascending ?
                       query.OrderBy(orderSelector) :
                       query.OrderByDescending(orderSelector);
            }

            var posao = query
                        .Skip((page - 1) * pagesize)
                        .FromSql("Select * FROM dbo.Vw_Posao")
                        .Take(pagesize)
                        .ToList();
            var model = new PosloviViewModel
            {
                Posao = posao,
                PagingInfo = pagingInfo
            };

            return View(model);

        }

        // GET: Posao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posao = await _context.Posao
                .Include(p => p.LokacijaPosla)
                .Include(p => p.Natjecaj)
                .Include(p => p.Usluga)
                .FirstOrDefaultAsync(m => m.PosaoId == id);
            if (posao == null)
            {
                return NotFound();
            }

            return View(posao);
        }
        [HttpGet]
        public IActionResult Create()
        {
            PrepareDropDownLists();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ViewPosao posaoView)
        {
            if (ModelState.IsValid)
            {
                
                try
                {

                   // return RedirectToAction("Index", "Usluga");
                    Posao posao = new Posao();

                    posao.Opis =             posaoView.Opis;
                    posao.Cijena =           posaoView.Cijena;
                    posao.DodatniTrosak =    posaoView.DodatniTrosak;
                    posao.VrijemeOd =        posaoView.VrijemeOd;
                    posao.VrijemeDo =        posaoView.VrijemeDo;
                    posao.UslugaId =         posaoView.UslugaId;
                    posao.LokacijaPoslaId =  posaoView.LokacijaPoslaId;

                   

                    _context.Add(posao);

                  //  PosaoRadnik pr = new PosaoRadnik();
                   // pr.PosaoId = posaoView.PosaoId;
                  //  pr.RadnikId = posaoView.RadnikId;

                  //  PosaoOprema po = new PosaoOprema();
//po.PosaoId = posaoView.PosaoId;
                  //  po.OpremaId = posaoView.OpremaId;

                   // _context.PosaoRadnik.Add(pr);
                   // _context.PosaoOprema.Add(po);

                    


                   
                    _context.SaveChanges();
                    logger.LogInformation($"Posao {posao.PosaoId} dodan.");
                    TempData[Constants.Message] = $"Posao {posao.PosaoId}dodan.";
                    TempData[Constants.ErrorOccurred] = false;

                   

                    return RedirectToAction("Create", "PosaoRadniks", new { posao.PosaoId, posaoView.KategorijaId, posaoView.OpremaId });

                }
                catch (Exception exc)
                {
                    logger.LogError("Pogreška prilikom dodavanje nove usluge: {0}", exc.CompleteExceptionMessage());
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(posaoView);
                }
            }
            else
            {
                PrepareDropDownLists();
                return View(posaoView);
            }

        }


        // GET: Usluga/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Vw_Posao
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.PosaoId == id);
            if (course == null)
            {
                return NotFound();
            }
            PrepareDropDownLists();
            return View(course);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(ViewPosao posaoView)
        {
            
            var courseToUpdate = await _context.Posao
                                .FirstOrDefaultAsync(c => c.PosaoId == posaoView.PosaoId);

            courseToUpdate.Cijena = posaoView.Cijena;
            courseToUpdate.Opis = posaoView.Opis;
            courseToUpdate.VrijemeOd = posaoView.VrijemeOd;
            courseToUpdate.VrijemeDo = posaoView.VrijemeDo;
            courseToUpdate.UslugaId = posaoView.UslugaId;
            _context.SaveChanges();
            try
            {
                PosaoRadnik pr = new PosaoRadnik();
                pr.RadnikId = posaoView.RadnikId;
                pr.PosaoId = posaoView.PosaoId;


                PosaoOprema po = new PosaoOprema();
                po.OpremaId = posaoView.OpremaId;
                pr.PosaoId = posaoView.PosaoId;
                _context.SaveChanges();
            }
            catch (DbUpdateException /* ex */)
            {
                ModelState.AddModelError("", "Neuspješno ažuriranje! ");
            }
            return RedirectToAction("Index", "Posao");

        }

        // GET: Posao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posao = await _context.Posao
                .Include(p => p.LokacijaPosla)
                .Include(p => p.Natjecaj)
                .Include(p => p.Usluga)
                .FirstOrDefaultAsync(m => m.PosaoId == id);
            if (posao == null)
            {
                return NotFound();
            }

            return View(posao);
        }

        // POST: Posao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var posao = await _context.Posao.FindAsync(id);
            _context.Posao.Remove(posao);
            await _context.SaveChangesAsync();
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

            var OpremaList = _context.Oprema
                      .AsNoTracking()
                      .ToList();
            ViewBag.OpremaList = new SelectList(OpremaList, nameof(Oprema.OpremaId), nameof(Oprema.Naziv));

            var ZanimanjeList = _context.Zanimanje
                     .AsNoTracking()
                      .ToList();
            ViewBag.ZanimanjeList = new SelectList(ZanimanjeList, nameof(Zanimanje.ZanimanjeId), nameof(Zanimanje.Naziv));

            var LokacijaList = _context.LokacijaPosla
                    .AsNoTracking()
                     .ToList();
            ViewBag.LokacijaList = new SelectList(LokacijaList, nameof(LokacijaPosla.LokacijaPoslaId), nameof(LokacijaPosla.NazivLokacije));


        }
    }
}
