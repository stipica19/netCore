using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ozo.Models
{
    public class ViewNajam
    {
        public int NajamId { get; set; }


        public string Opis { get; set; }
        [Required(ErrorMessage = "Firma je obavezno polje!")]
        [Display(Name = "Firma ")]
        public int FimraId { get; set; }
        [Required(ErrorMessage = "Vrsta najma je obavezno polje!")]
        [Display(Name = "Vrsta najma")]
        public int VrstaNajmaId { get; set; }
        [Required(ErrorMessage = "Datum od je obavezno polje!")]
        public DateTime DatumOd { get; set; }
        [Required(ErrorMessage = "Datum do je obavezno polje!")]
        public DateTime DatumDo { get; set; }

        public string NazivOpreme { get; set; }
      
        public string VrstaNajma { get; set; }
        [Required(ErrorMessage = "Kolicina je obavezno polje!")]
        public int Kolicina { get; set; }
        [Required(ErrorMessage = "Cijena je obavezno polje!")]
        public int Cijena { get; set; }
        [Required(ErrorMessage = "Oprema je obavezno polje!")]
        [Display(Name = "Oprema")]
        public int OpremaId { get; set; }

        public string NazivFirme { get; set; }




    }
}
