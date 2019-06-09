using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class ReferentniTipOpreme
    {
        public ReferentniTipOpreme()
        {
            Oprema = new HashSet<Oprema>();
            UslugaOprema = new HashSet<UslugaOprema>();
        }

        public int ReferentniTipOpremeId { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public int TipOpremeId { get; set; }

        public Registar TipOpreme { get; set; }
        public ICollection<Oprema> Oprema { get; set; }
        public ICollection<UslugaOprema> UslugaOprema { get; set; }
    }
}
