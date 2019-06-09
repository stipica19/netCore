using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ozo.Models
{
    public partial class Osoba
    {
        public Osoba()
        {
            OsobaCertifikat = new HashSet<OsobaCertifikat>();
            Radnik = new HashSet<Radnik>();
            Servis = new HashSet<Servis>();
        }
        
        public int OsobaId { get; set; }
        [Display(Name = "Ime")]
        public string Ime { get; set; }
        [Required(ErrorMessage = "Prezime je obavezno polje!")]
        [Display(Name = "Prezime")]
        public string Prezime { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? GodRodjenja { get; set; }
        //public int CertifikatId { get; set; }

        public ICollection<OsobaCertifikat> OsobaCertifikat { get; set; }
        public ICollection<Radnik> Radnik { get; set; }
        public ICollection<Servis> Servis { get; set; }
    }
}
