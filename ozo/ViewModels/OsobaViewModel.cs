using ozo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ozo.ViewModels
{
    public class OsobaViewModel
    {
       
       public int OsobaId { get; set; }

        public string Ime { get; set; }

        public string Prezime { get; set; }

        public DateTime? God_rodjenja { get; set; }

        public int CertifikatId { get; set; }

        public int ZanimanjeId { get; set; }

        public string NazivZanimanja { get; set; }
        public string NazivCertifikata { get; set; }
    }
}
