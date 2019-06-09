using ozo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ozo.ViewModels
{
    public class UslugaViewModel
    {


        public int UslugaId { get; set; }
        public int ZanId { get; set; }
        public int ZanimanjeId { get; set; }
        public int ReferentniTipOpremeId { get; set; }
        public int KategorijaPoslaId { get; set; }
        public string NazivTipaZanimanja { get; set; }
        public string NazivTipaOpreme { get; set; }
        public string Opis { get; set; }
        public string Naziv { get; set; }
        public string KategorijaPoslaNaziv { get; set; }

    }
}
