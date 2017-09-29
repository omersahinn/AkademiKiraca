using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AkademiKiraca.Models
{
    public class UserMessageModel
    {
       
        [Display(Name = "Ad Soyad")]
        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        public string nameSurname { get; set; }

      
        [Display(Name = "Telefon")]
        [StringLength(13, ErrorMessage = "13 Karakterden Fazla Olamaz")]
        public string phone { get; set; }

        [EmailAddress(ErrorMessage ="Geçersiz E-Mail Adresi ")]
        [Required(ErrorMessage ="Bu Alan Boş Geçilemez")]
        [Display(Name = "E-Mail")]
        [StringLength(100,ErrorMessage ="100 Karakterden Fazla Olamaz")]
        public string email { get; set; }

        [Display(Name = "Mesajınız")]
        public string message { get; set; }

        public DateTime? dateTime { get; set; }
    }
}