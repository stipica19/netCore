using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ozo.Models
{
    public partial class Posao
    {
        public Posao()
        {
            PosaoOprema = new HashSet<PosaoOprema>();
            PosaoRadnik = new HashSet<PosaoRadnik>();
        }

        public int PosaoId { get; set; }
        public string Opis { get; set; }
        public int Cijena { get; set; }
        public int? DodatniTrosak { get; set; }
        public DateTime VrijemeOd { get; set; }
        public DateTime VrijemeDo { get; set; }
        public int? UslugaId { get; set; }
      
      

        public virtual Usluga Usluga { get; set; }
        public int LokacijaPoslaId { get; set; }
        public int? NatjecajId { get; set; }

        public LokacijaPosla LokacijaPosla { get; set; }
        public virtual Natjecaj Natjecaj { get; set; }
     //   public Usluga Usluga { get; set; }
        public ICollection<PosaoOprema> PosaoOprema { get; set; }
        public ICollection<PosaoRadnik> PosaoRadnik { get; set; }
    }
}
