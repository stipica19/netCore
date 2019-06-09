using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ozo.Models;

namespace ozo.Controllers.AutoComplete
{
    [Route("autocomplete/[controller]")]
    public class ServisController : Controller
    {
        private readonly PI01Context ctx;
        private readonly AppSettings appData;

        public ServisController(PI01Context ctx, IOptions<AppSettings> options)
        {
            this.ctx = ctx;
            appData = options.Value;
        }

        [HttpGet]
        public IEnumerable<IdLabel> Get(string term)
        {
            var query = ctx.Servis
                    .Include(b=>b.Oprema)
                            .Select(v => new IdLabel
                            {
                                Id = v.OpremaId,
                                Label = v.Oprema.Naziv
                            })
                            .Where(l => l.Label.Contains(term));

            //var q = ctx.Servis
            //     .Include(b => b.Osoba)
            //             .Select(v => new IdLabel
            //             {
            //                 Id = v.OsobaId,
            //                 Label = v.Osoba.Ime
            //             })
            //             .Where(l => l.Label.Contains(term));

            var list = query.OrderBy(l => l.Label)
                            .ThenBy(l => l.Id)
                            .ToList();
         
            
            return list;
        }
    }
}
