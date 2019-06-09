using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ozo.Models;
using Microsoft.Extensions.Options;


namespace ozo.Controllers.AutoComplete
{
        [Route("autocomplete/[controller]")]
        public class OsobaController : Controller
        {
            private readonly PI01Context _context;
            private readonly AppSettings appData;

            public OsobaController(PI01Context ctx, IOptions<AppSettings> options)
            {
             this._context = ctx;
             appData = options.Value;
        }

        public IOptions<AppSettings> Options { get; }

        [HttpGet]
            public IEnumerable<IdLabel> Get(string term)
            {
                var query = _context.Vw_Osoba
                                .Select(o => new IdLabel
                                {
                                    Id = o.ZanimanjeId,
                                    Label = o.NazivZanimanja
                                })
                                .Where(l => l.Label.Contains(term));

                var list = query.OrderBy(l => l.Label)
                                .ThenBy(l => l.Id)
                                .ToList();
                return list;
            }
        }
    }

