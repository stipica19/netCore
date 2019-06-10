using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ozo.Models
{
    public class ViewUsluga
    {

        public int UslugaId { get; set; }
        public int ZanId { get; set; }
        [Required(ErrorMessage = "Zanimanje  je obavezno polje!")]
        [Display(Name = "Zanimanje")]
        public int ZanimanjeId { get; set; }



        [Required(ErrorMessage = "Referenti Tip Opreme je obavezno polje!")]
        [Display(Name = "Referenti Tip Opreme")]
        public int ReferentniTipOpremeId { get; set; }
        [Required(ErrorMessage = "Kategorija posla  je obavezno polje!")]
        [Display(Name = "Kategorija posla")]
        public int KategorijaPoslaId { get; set; }
        public string NazivTipaZanimanja {get;set;}
        public string NazivTipaOpreme { get; set; }
        public string Opis { get; set; }
        public string Naziv { get; set; }
        public string KategorijaPoslaNaziv { get; set; }



    }
}
