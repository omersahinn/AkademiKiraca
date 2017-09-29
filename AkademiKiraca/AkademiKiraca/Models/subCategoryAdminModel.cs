using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AkademiKiraca.Models
{
    public class subCategoryAdminModel
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Kategori Adı")]
        public string subCategoryName { get; set; }
   
        [AllowHtml]
        [UIHint("tinymce_full_compressed")]
        [Display(Name = "İçerik")]
        public string description { get; set; }

        public IList<Picture> picture { get; set; }
        public DateTime? createdDate { get; set; }
        public string categoryName { get; set; }



    }
}