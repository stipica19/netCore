using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class LokacijaPosla
    {
        public LokacijaPosla()
        {
            Posao = new HashSet<Posao>();
        }

        public int LokacijaPoslaId { get; set; }
        public string NazivLokacije { get; set; }
        public int GradId { get; set; }

        public Registar Grad { get; set; }
        public ICollection<Posao> Posao { get; set; }
    }
}
