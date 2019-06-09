using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class UslugaLjudi
    {
        public int UslugaLjudiId { get; set; }
        public int ZanimanjeId { get; set; }
        public int UslugaId { get; set; }

        public Usluga Usluga { get; set; }
        public Zanimanje Zanimanje { get; set; }
    }
}
