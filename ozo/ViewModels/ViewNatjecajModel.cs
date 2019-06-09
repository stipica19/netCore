using ozo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ozo.ViewModels
{
    public class ViewNatjecajModel
    {
        public IEnumerable<Natjecaj> Natjecaj { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
