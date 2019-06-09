using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class NatjecajFirma
    {
        public int NatjecajFirmaId { get; set; }
        public int NatjecajId { get; set; }
        public int FirmaId { get; set; }
        public int? Ponuda { get; set; }

        public Registar Firma { get; set; }
        public Natjecaj Natjecaj { get; set; }
    }
}
