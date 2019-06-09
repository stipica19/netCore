using System;



using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ozo.Models;

namespace ozo.ViewModels
{
    public class OpremaViewModel
    {
        public IEnumerable<ViewOprema> Oprema { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
