using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class TipRegistra
    {
        public TipRegistra()
        {
            Registar = new HashSet<Registar>();
        }

        public int TipRegistraId { get; set; }
        public string Naziv { get; set; }

        public ICollection<Registar> Registar { get; set; }
    }
}
