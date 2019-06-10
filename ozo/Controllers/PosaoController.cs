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

        // GET: Usluga
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


                    Posao posao = new Posao();

                    posao.Opis = posaoView.Opis;
                    posao.Cijena = posaoView.Cijena;
                    posao.DodatniTrosak = posaoView.DodatniTrosak;
                    posao.VrijemeOd = posaoView.VrijemeOd;
                    posao.VrijemeDo = posaoView.VrijemeDo;
                    posao.UslugaId = posaoView.UslugaId;
                    //posao.NatjecajId = posaoView.NatjecajId;
                   posao.LokacijaPoslaId = posaoView.LokacijaPoslaId;

                    Console.WriteLine("idee" + posaoView.PosaoId);

                    _context.Add(posao);

                    PosaoRadnik pr = new PosaoRadnik();
                    pr.PosaoId = posao.PosaoId;
                    pr.RadnikId = posaoView.RadnikId;

                    _context.PosaoRadnik.Add(pr);


                    PosaoOprema po = new PosaoOprema();
                    po.PosaoId = posao.PosaoId;
                    po.OpremaId = posaoView.OpremaId;
                    _context.PosaoOprema.Add(po);

                    


                   
                    _context.SaveChanges();
                    logger.LogInformation($"Posao {posao.PosaoId} dodan.");
                    TempData[Constants.Message] = $"Posao {posao.PosaoId}dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));

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


        // GET: Posao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posao = await _context.Posao.FindAsync(id);
            if (posao == null)
            {
                return NotFound();
            }
            ViewData["LokacijaPoslaId"] = new SelectList(_context.LokacijaPosla, "LokacijaPoslaId", "NazivLokacije", posao.LokacijaPoslaId);
            ViewData["NatjecajId"] = new SelectList(_context.Natjecaj, "NatjecajId", "Naziv", posao.NatjecajId);
            ViewData["UslugaId"] = new SelectList(_context.Usluga, "UslugaId", "Naziv", posao.UslugaId);
            return View(posao);
        }

        // POST: Posao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PosaoId,Opis,Cijena,DodatniTrosak,VrijemeOd,VrijemeDo,UslugaId,LokacijaPoslaId,NatjecajId")] Posao posao)
        {
            if (id != posao.PosaoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(posao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PosaoExists(posao.PosaoId))
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
            ViewData["LokacijaPoslaId"] = new SelectList(_context.LokacijaPosla, "LokacijaPoslaId", "NazivLokacije", posao.LokacijaPoslaId);
            ViewData["NatjecajId"] = new SelectList(_context.Natjecaj, "NatjecajId", "Naziv", posao.NatjecajId);
            ViewData["UslugaId"] = new SelectList(_context.Usluga, "UslugaId", "Naziv", posao.UslugaId);
            return View(posao);
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
