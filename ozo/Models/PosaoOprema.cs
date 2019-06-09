using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class PosaoOprema
    {
        public int PosaoOpremaId { get; set; }
        public int OpremaId { get; set; }
        public int PosaoId { get; set; }

        public Oprema Oprema { get; set; }
        public Posao Posao { get; set; }
    }
}
