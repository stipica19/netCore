using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ozo.Models
{
    public class ViewPosao
    {

        public int PosaoId { get; set; }

        [Required(ErrorMessage = "opis je obavezno polje!")]
        public string Opis { get; set; }
        [Required(ErrorMessage = "Cijena je obavezno polje!")]
        public int Cijena { get; set; }

        public int DodatniTrosak { get; set; }
        [Required(ErrorMessage = "Datum od je obavezno polje!")]
        public DateTime VrijemeOd { get; set; }
        [Required(ErrorMessage = "Datum do je obavezno polje!")]
        public DateTime VrijemeDo { get; set; }
        [Required(ErrorMessage = "usluga je obavezno polje!")]
        [Display(Name = "Usluga")]
        public int UslugaId { get; set; }
        public string NazivZanimanja { get; set; }
        public string NazivOpreme { get; set; }
        [Required(ErrorMessage = "Zanimanje je obavezno polje!")]
        [Display(Name = "Zanimanje")]
        public int KategorijaId { get; set; }
        [Required(ErrorMessage = "Oprema je obavezno polje!")]
        [Display(Name = "Oprema")]
        public int OpremaId { get; set; }
        public string NazivUsluge { get; set; }
        public string NazivLokacije { get; set; }
        [Required(ErrorMessage = "Lokacija je obavezno polje!")]
        [Display(Name = "Lokacija Posla")]
        public int LokacijaPoslaId{ get; set; }
       // public int NatjecajId { get; set; }
        public int RadnikId { get; set; }

       


    }
}
