using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AkademiKiraca.Models
{
    public class changePasswordModel
    {
        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [Display(Name = "Eski Şifre")]
        public string oldPassword { get; set; }
        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [Display(Name = "Yeni Şifre")]
        public string newPassword { get; set; }
        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [Display(Name = "Şifreyi Doğrula")]
        public string confirmPassword { get; set; }
    }
}