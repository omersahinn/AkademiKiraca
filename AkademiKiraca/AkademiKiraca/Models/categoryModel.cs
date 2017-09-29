using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkademiKiraca.Models
{
    public class categoryModel
    {
        public int id { get; set; }

        public string categoryName { get; set; }

        public ICollection<SubCategory> subCategoryName { get; set; }


    }
}