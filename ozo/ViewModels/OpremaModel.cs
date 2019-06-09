using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ozo.Models;

namespace ozo.ViewModels
{
    public class OpremaModel
    {
        public IEnumerable<Oprema> Oprema { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}