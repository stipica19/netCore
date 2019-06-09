using ozo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ozo.ViewModels
{
    public class NajmoviViewModel
    {

        public IEnumerable<ViewNajam> Najam { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
