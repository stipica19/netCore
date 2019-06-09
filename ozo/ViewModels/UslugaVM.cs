using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ozo.ViewModels
{
    public class UslugaVM
    {
        
        public int UslugaId { get; set; }
        [Required(ErrorMessage = "Id je obavezno polje!")]
       

        public int ZanimanjeId { get; set; }
        public int KategorijaPoslaId { get; set; }



        public string Opis { get; set; }
        public string Naziv { get; set; }


    }
}
