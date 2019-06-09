using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class Servis
    {
        public int ServisId { get; set; }
        public int OpremaId { get; set; }
        public int ServiserId { get; set; }
        public int OsobaId { get; set; }
        public int Cijena { get; set; }
        public string Opis { get; set; }

        public Oprema Oprema { get; set; }
        public Osoba Osoba { get; set; }
        public Registar Serviser { get; set; }
    }
}
