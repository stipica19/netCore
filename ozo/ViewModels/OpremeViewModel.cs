using ozo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ozo.ViewModels
{
    public class OpremeViewModel
    {

        public int OpremaId { get; set; }
        public int InventarniBroj { get; set; }
        public string Naziv { get; set; }
        public int? KnjigovostvenaVrijednost { get; set; }
        public int? NabavnaCijena { get; set; }
        public int ReferentniTipOpremeId { get; set; }
        public int LokacijaOpremeId { get; set; }
        public int StatusId { get; set; }
      

    }
}
