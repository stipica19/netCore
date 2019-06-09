using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace ozo.Models
{
    public class ViewPosao
    {
        

        public int PosaoId { get; set; }
        public string Opis { get; set; }
        public int Cijena { get; set; }
        //public int? DodatniTrosak { get; set; }
        public DateTime VrijemeOd { get; set; }
        public DateTime VrijemeDo { get; set; }
        public string NazivUsluge { get; set; }    
        public string NazivLokacije { get; set; }
        public string NazivNatjecaja { get; set; }
        
       // public string NazivOpreme { get; set; } //Naziv opreme
   
       


    }
}
