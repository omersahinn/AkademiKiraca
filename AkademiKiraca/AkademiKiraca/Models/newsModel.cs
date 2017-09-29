using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AkademiKiraca.Models
{
    public class newsModel
    {

        public int id { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [Display(Name = "Başlık")]
        public string title { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [Display(Name = "İçerik")]
        public string contentNews { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [Display(Name = "Kısa Başlık")]
        public string shortTitle { get; set; }

        public IEnumerable<Picture> newsPicture { get; set; }
    }
}