using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class Radnik
    {
        public Radnik()
        {
            PosaoRadnik = new HashSet<PosaoRadnik>();
        }

        public int RadnikId { get; set; }
        public int OsobaId { get; set; }
        public int KategorijaId { get; set; }

        public Zanimanje Kategorija { get; set; }
        public Osoba Osoba { get; set; }
        public ICollection<PosaoRadnik> PosaoRadnik { get; set; }
    }
}
