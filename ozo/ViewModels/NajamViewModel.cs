using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ozo.ViewModels
{
    public class NajamViewModel
    {
        public int NajamId { get; set; }
        public string Opis { get; set; }
        public int FimraId { get; set; }
        public int VrstaNajmaId { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }

        public string NazivOpreme { get; set; }
        public string VrstaNajma { get; set; }
        public int Kolicina { get; set; }
        public int Cijena { get; set; }
        public int OpremaId { get; set; }
        public int NazivFirme { get; set; }

    }


}
