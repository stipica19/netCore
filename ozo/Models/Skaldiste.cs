using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class Skaldiste
    {
        public Skaldiste()
        {
            LokacijaOpreme = new HashSet<LokacijaOpreme>();
        }

        public int SkladisteId { get; set; }
        public string NazivSkladista { get; set; }
        public int GradId { get; set; }

        public Registar Grad { get; set; }
        public ICollection<LokacijaOpreme> LokacijaOpreme { get; set; }
    }
}
