using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ozo.Models
{
    public partial class Servis
    {
        public int ServisId { get; set; }
        [Required(ErrorMessage = "Oprema je obavezno polje!")]
        [Display(Name = "Oprema")]
        public int OpremaId { get; set; }
        [Required(ErrorMessage = "Serviser je obavezno polje!")]
        [Display(Name = "Serviser")]
        public int ServiserId { get; set; }
        [Required(ErrorMessage = "Osoba je obavezno polje!")]
        [Display(Name = "Osoba")]
        public int OsobaId { get; set; }
        [Required(ErrorMessage = "Cijena je obavezno polje!")]
        [Display(Name = "Cijena")]
        public int Cijena { get; set; }
        [Required(ErrorMessage = "Opis je obavezno polje!")]
        [Display(Name = "Opis")]
        public string Opis { get; set; }

        public Oprema Oprema { get; set; }
        public Osoba Osoba { get; set; }
        public Registar Serviser { get; set; }
    }
}
