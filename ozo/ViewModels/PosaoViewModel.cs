using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ozo.ViewModels
{
    public class PosaoViewModel
    {
        public int PosaoId { get; set; }
        public string Opis { get; set; }
        public int Cijena { get; set; }
        public int? DodatniTrosak { get; set; }
        public DateTime VrijemeOd { get; set; }
        public DateTime VrijemeDo { get; set; }
        public int? UslugaId { get; set; }
        public string NazivZanimanja { get; set; }
        public string NazivOpreme { get; set; }
        public int KategorijaId { get; set; }
        public int OpremaId { get; set; }
        public string NazivUsluge { get; set; }
    }
}
