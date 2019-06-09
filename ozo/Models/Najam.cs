using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class Najam
    {
        public Najam()
        {
            OpremaStavka = new HashSet<OpremaStavka>();
        }

        public int NajamId { get; set; }
        public string Opis { get; set; }
        public int FimraId { get; set; }
        public int VrstaNajmaId { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }

        public Registar Fimra { get; set; }
        public Registar VrstaNajma { get; set; }
        public ICollection<OpremaStavka> OpremaStavka { get; set; }
    }
}
