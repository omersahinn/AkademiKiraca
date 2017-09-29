using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AkademiKiraca.Models
{
    public class adminModel
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [Display(Name = "Ad Soyad")]
        public string nameSurname { get; set; }

        [StringLength(100, ErrorMessage = "100 Karakterden Fazla Olamaz")]
        [EmailAddress(ErrorMessage = "Geçersiz E-Mail Adresi ")]
        [Required(ErrorMessage = "Bu Alan Boş Geçilemez")]
        [Display(Name = "E-Mail")]
        public string eMail { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [Display(Name = "Şifre")]
        public string password { get; set; }
    }
}