using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ozo.Models
{
    public partial class Oprema
    {
        public Oprema()
        {
            OpremaStavka = new HashSet<OpremaStavka>();
            PosaoOprema = new HashSet<PosaoOprema>();
            Servis = new HashSet<Servis>();
        }
        



        public int OpremaId { get; set; }
        public int InventarniBroj { get; set; }

        [Required(ErrorMessage = "Ime opreme je obvezno polje")]
        [Display(Name = "Ime opreme")]
        public string Naziv { get; set; }
        public int? KnjigovostvenaVrijednost { get; set; }
        [Display(Name = "Referentni Tip Opreme ")]
        public int ReferentniTipOpremeId { get; set; }
        [Display(Name = "Lokacija Opreme")]
        public int LokacijaOpremeId { get; set; }
        [Display(Name = "Status")]
        public int StatusId { get; set; }
        [Display(Name = "Cijena nabave")]
        public int? NabavnaCijena { get; set; }


        public LokacijaOpreme LokacijaOpreme { get; set; }
        public ReferentniTipOpreme ReferentniTipOpreme { get; set; }
        public Registar Status { get; set; }
        public ICollection<OpremaStavka> OpremaStavka { get; set; }
        public ICollection<PosaoOprema> PosaoOprema { get; set; }
        public ICollection<Servis> Servis { get; set; }
    }
}
