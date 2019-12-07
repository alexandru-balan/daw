using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YahooGroups.Models
{
    public class CategoryModel
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }

        // A list of groups that belong to this category
    }
}