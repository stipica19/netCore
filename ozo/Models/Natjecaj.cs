using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class Natjecaj
    {
        public Natjecaj()
        {
            
            Posao = new HashSet<Posao>();
        }

        public int NatjecajId { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public int? Vrijednost { get; set; }
        public DateTime? VrijemeOd { get; set; }
        public DateTime? VrijemeDo { get; set; }
        public int VrstaNatjecajaId { get; set; }
        public int RaspisateljId { get; set; }
        public int PobiednikId { get; set; }

        public Registar Pobiednik { get; set; }
        public Registar Raspisatelj { get; set; }
        public Registar VrstaNatjecaja { get; set; }
        
        public ICollection<Posao> Posao { get; set; }
    }
}
