using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkademiKiraca.Models
{
    public class subCategoryModel
    {
        public int id { get; set; }
        public string subCategoryName { get; set; }
        public string description { get; set; }
        public int? position { get; set; }
        public string picturePath { get; set; }
        public DateTime? updatedDate { get; set; }
        public string categoryName { get; set; }


    }
}