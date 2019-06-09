using ozo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ozo.ViewModels
{
    public class UslugeViewModel
    {

        public IEnumerable<ViewUsluga> Usluga { get; set; }
        public PagingInfo PagingInfo { get; set; }


    }
}
