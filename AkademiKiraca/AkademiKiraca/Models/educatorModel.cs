using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AkademiKiraca.Models
{
    public class educatorModel
    {
        public int id { get; set; }
        [Display(Name = "Ad Soyad")]
        public string nameSurname { get; set; }
        [Display(Name = "Şirket")]
        public string company { get; set; }
        [Display(Name = "Hakkında")]
        public string about { get; set; }
        [Display(Name = "Profil Resim")]
        public string picturePath { get; set; }
       
    }
}