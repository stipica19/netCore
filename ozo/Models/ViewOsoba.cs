using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ozo.Models
{
    public class ViewOsoba
    {

      public int OsobaId { get; set; }
        [Required(ErrorMessage = "Ime je obavezno polje!")]
        [Display(Name =  "Ime")]
        public string Ime { get; set; }
        [Required(ErrorMessage = "Prezime je obavezno polje!")]
        [Display(Name = "Prezime")]
        public string Prezime { get; set; }

        [Display(Name = "Godina rodjenja")]
        public DateTime? God_rodjenja { get; set; }

        [Required(ErrorMessage = "Certifikat je obavezno polje!")]
        [Display(Name = "Certifikat")]
        public int CertifikatId { get; set; }

        [Required(ErrorMessage = "Zanimanje je obavezno polje!")]
        [Display(Name = "Zanimanje")]
        public int ZanimanjeId { get; set; }
        public int KategorijaId { get; set; }

        public string NazivZanimanja { get; set; }
        public string NazivCertifikata { get; set; }
        
    }
}
