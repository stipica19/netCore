using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class UslugaOprema
    {
        public int UslugaOpremaId { get; set; }
        public int UslugaId { get; set; }
        public int ReferentniTipOpremeId { get; set; }

        public ReferentniTipOpreme ReferentniTipOpreme { get; set; }
        public Usluga Usluga { get; set; }
    }
}
