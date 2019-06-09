using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class KategorijaPosla
    {
        public KategorijaPosla()
        {
            Usluga = new HashSet<Usluga>();
        }

        public int KategorijaPoslaId { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public int? Cijena { get; set; }

        public ICollection<Usluga> Usluga { get; set; }
    }
}
