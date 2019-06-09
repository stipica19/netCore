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

        [Display(Name =  "Ime")]
        public string Ime { get; set; }
        [Required(ErrorMessage = "Prezime je obavezno polje!")]
        [Display(Name = "Prezime")]
        public string Prezime { get; set; }
       
        public DateTime? God_rodjenja { get; set; }

        public int CertifikatId { get; set; }

        public int ZanimanjeId { get; set; }
        public int KategorijaId { get; set; }

        public string NazivZanimanja { get; set; }
        public string NazivCertifikata { get; set; }
        
    }
}
