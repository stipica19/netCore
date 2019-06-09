using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class OpremaStavka
    {
        public int OpremaStavkaId { get; set; }
        public int Kolicina { get; set; }
        public int Cijena { get; set; }
        public int OpremaId { get; set; }
        public int NajamId { get; set; }

        public Najam Najam { get; set; }
        public Oprema Oprema { get; set; }
    }
}
