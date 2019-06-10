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
    public class OpremaController : Controller
    {
        private readonly PI01Context ctx;
        private readonly AppSettings appData;

        public OpremaController(PI01Context ctx, IOptions<AppSettings> options)
        {
            this.ctx = ctx;
            appData = options.Value;
        }

        [HttpGet]
        public IEnumerable<IdLabel> Get(string term)
        {
            var query = ctx.Oprema
                            .Select(v => new IdLabel
                            {
                                Id = v.ReferentniTipOpremeId,
                                Label = v.ReferentniTipOpreme.Naziv
                            })
                            .Where(l => l.Label.Contains(term));

   

            var list = query.OrderBy(l => l.Label)
                            .ThenBy(l => l.Id)
                            .ToList();


            return list;
        }
    }
}
