using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class Zanimanje
    {
        public Zanimanje()
        {
            Radnik = new HashSet<Radnik>();
            UslugaLjudi = new HashSet<UslugaLjudi>();
        }

        public int ZanimanjeId { get; set; }
        public string Naziv { get; set; }
        public int? Cijena { get; set; }
        public int? Placa { get; set; }
        public string Opis { get; set; }

        public ICollection<Radnik> Radnik { get; set; }
        public ICollection<UslugaLjudi> UslugaLjudi { get; set; }
    }
}
