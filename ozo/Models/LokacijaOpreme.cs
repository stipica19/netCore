using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class LokacijaOpreme
    {
        public LokacijaOpreme()
        {
            Oprema = new HashSet<Oprema>();
        }

        public int LokacijaOpremeId { get; set; }
        public string NazivLokacije { get; set; }
        public int SkladisteId { get; set; }

        public Skaldiste Skladiste { get; set; }
        public ICollection<Oprema> Oprema { get; set; }
    }
}
