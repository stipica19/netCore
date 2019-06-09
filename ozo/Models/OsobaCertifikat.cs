using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class OsobaCertifikat
    {
        public int OsobaCertifikatId { get; set; }
        public int OsobaId { get; set; }
        public int CertifikatId { get; set; }

        public Registar Certifikat { get; set; }
        public Osoba Osoba { get; set; }
    }
}
