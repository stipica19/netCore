using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ozo.Models
{
    public class ViewOprema
    {

        public int OpremaId { get; set; }
        [Display(Name = "Inventarni broj")]
        public int InventarniBroj { get; set; }


        [Display(Name = "Naziv")]
        public string Naziv { get; set; }


        [Display(Name = "Knjigovostvena vrijednost")]
        public int? KnjigovostvenaVrijednost { get; set; }


        [Display(Name = "Nabavna cijena")]
        public int? NabavnaCijena { get; set; }

        [Display(Name = "Referentni Tip Opreme")]
        public int ReferentniTipOpremeId { get; set; }

        [Display(Name = "Lokacija Opreme")]
        public int LokacijaOpremeId { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }


        public string NazivRef { get; set; }


        public string NazivStatus { get; set; }


        public string NazivLokacije { get; set; }
    }
}
