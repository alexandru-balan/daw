using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YahooGroups.Models
{
    public class CategoryModel
    {
        [Required (AllowEmptyStrings = false, ErrorMessage = "A category must have a name")]
        public string Name { get; set; }

        [Key]
        public int CategoryId { get; set; }

        // A list of groups that belong to this category
    }
}