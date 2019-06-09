using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class Usluga
    {
        public Usluga()
        {
            Posao = new HashSet<Posao>();
            UslugaLjudi = new HashSet<UslugaLjudi>();
            UslugaOprema = new HashSet<UslugaOprema>();
        }

        public int UslugaId { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public int KategorijaPoslaId { get; set; }

        public KategorijaPosla KategorijaPosla { get; set; }
        public ICollection<Posao> Posao { get; set; }
        public ICollection<UslugaLjudi> UslugaLjudi { get; set; }
        public ICollection<UslugaOprema> UslugaOprema { get; set; }
    }
}
