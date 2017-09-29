using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkademiKiraca.Models
{
    public class viewModel
    {
        public IEnumerable<subCategoryModel> subCategory { get; set; }
        public UserMessageModel userMessage { get; set; }
    }
}