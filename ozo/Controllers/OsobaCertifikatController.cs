using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ozo.Models;

namespace ozo.Controllers
{
    public class OsobaCertifikatController : Controller
    {
        private readonly PI01Context _context;
        private readonly AppSettings appData;

        public OsobaCertifikatController(PI01Context context, IOptions<AppSettings> options)
        {
            _context = context;
            appData = options.Value;
        }
    

    // GET: OsobaCertifikat
    public IActionResult Index()
    {
        var osobe = _context.OsobaCertifikat
                        .AsNoTracking()
                        .Include(c=>c.Osoba)
                        .Include(c=>c.Certifikat)
                       // .OrderBy(d => d)
                        .ToList();
        return View("Index", osobe);
    }

      
        public IActionResult Create(int osobaId, int certifikatId, int zanimanjeId)
    {
        if (ModelState.IsValid)
        {
            try
            {
                    OsobaCertifikat osobaCert = new OsobaCertifikat();
                    osobaCert.CertifikatId = certifikatId;
                    osobaCert.OsobaId = osobaId;


                _context.OsobaCertifikat.Add(osobaCert);
                _context.SaveChanges();
                    //logger.LogInformation($"Osoba {osoba.Ime} dodana.");
                    // TempData[Constants.Message] = $"Osoba {} dodana.";
                    //  TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction("Create", "Radnik", new { osobaId, zanimanjeId });

                }
                catch (Exception)
            {
                //  logger.LogError("Pogreška prilikom dodavanje nove osobe: {0}", exc.CompleteExceptionMessage());
                //  ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                return View();
            }
        }
        else
            {
                PrepareDropDownLists();
                return View();
        }

    }


        public async Task<IActionResult> EditPost(int osobaId, int certifikatId, int zanimanjeId)
        {

            Console.WriteLine("govno" + certifikatId);


            if (osobaId == null)
            {
                return NotFound();
            }

            var courseToUpdate = await _context.OsobaCertifikat
                .FirstOrDefaultAsync(c => c.OsobaId == osobaId);

            if (await TryUpdateModelAsync<OsobaCertifikat>(courseToUpdate,
                "",
                c=>c.OsobaId,
                c=>c.CertifikatId
                ))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    

                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Neuspješno ažuriranje! ");
                }
                Console.WriteLine("mag" + zanimanjeId);
                return RedirectToAction("EditPost", "Radnik", new { osobaId, zanimanjeId });
            }
            return View(courseToUpdate);
        }





        [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int OsobaCertifikatId)
    {
        var osoba = _context.OsobaCertifikat.Find(OsobaCertifikatId);
        if (osoba != null)
        {
            try
            {
                int naziv = osoba.OsobaCertifikatId;
                _context.Remove(osoba);
                _context.SaveChanges();
                //logger.LogInformation($"oprema {naziv} uspješno obrisan.");
                TempData[Constants.Message] = "Uspjesno obrisana osoba: ";
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
            TempData[Constants.Message] = "Ne postoji oprema s oznakom: " ;
            TempData[Constants.ErrorOccurred] = true;
        }
        return RedirectToAction(nameof(Index));
    }

    private bool OsobaCertifikatExists(int id)
        {
            return _context.OsobaCertifikat.Any(e => e.OsobaCertifikatId == id);
        }
        private void PrepareDropDownLists()
        {

            var StatusOpreme = _context.Registar
                .AsNoTracking()
                        .FromSql("SELECT * FROM dbo.Registar")
                        .Where(c => c.TipRegistraId == 1)
                        //.Where(c => c.Naziv == "Nedostupno")



                        .ToList();
            ViewBag.StatusOpreme = new SelectList(StatusOpreme, nameof(Registar.RegistarId), nameof(TipRegistra.Naziv));


        }
    }
}
