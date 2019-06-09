using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class Registar
    {
        public Registar()
        {
            LokacijaPosla = new HashSet<LokacijaPosla>();
            NajamFimra = new HashSet<Najam>();
            NajamVrstaNajma = new HashSet<Najam>();
           // NatjecajFirma = new HashSet<NatjecajFirma>();
            NatjecajPobiednik = new HashSet<Natjecaj>();
            NatjecajRaspisatelj = new HashSet<Natjecaj>();
            NatjecajVrstaNatjecaja = new HashSet<Natjecaj>();
            Oprema = new HashSet<Oprema>();
            OsobaCertifikat = new HashSet<OsobaCertifikat>();
            ReferentniTipOpreme = new HashSet<ReferentniTipOpreme>();
            Servis = new HashSet<Servis>();
            Skaldiste = new HashSet<Skaldiste>();
        }

        public int RegistarId { get; set; }
        public string Naziv { get; set; }
        public int TipRegistraId { get; set; }

        public TipRegistra TipRegistra { get; set; }
        public ICollection<LokacijaPosla> LokacijaPosla { get; set; }
        public ICollection<Najam> NajamFimra { get; set; }
        public ICollection<Najam> NajamVrstaNajma { get; set; }
       // public ICollection<NatjecajFirma> NatjecajFirma { get; set; }
        public ICollection<Natjecaj> NatjecajPobiednik { get; set; }
        public ICollection<Natjecaj> NatjecajRaspisatelj { get; set; }
        public ICollection<Natjecaj> NatjecajVrstaNatjecaja { get; set; }
        public ICollection<Oprema> Oprema { get; set; }
        public ICollection<OsobaCertifikat> OsobaCertifikat { get; set; }
        public ICollection<ReferentniTipOpreme> ReferentniTipOpreme { get; set; }
        public ICollection<Servis> Servis { get; set; }
        public ICollection<Skaldiste> Skaldiste { get; set; }
    }
}
