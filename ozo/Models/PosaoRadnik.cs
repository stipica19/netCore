using System;
using System.Collections.Generic;

namespace ozo.Models
{
    public partial class PosaoRadnik
    {
        public int RadnikPosaoId { get; set; }
        public int RadnikId { get; set; }
        public int PosaoId { get; set; }

        public Posao Posao { get; set; }
        public Radnik Radnik { get; set; }
    }
}
