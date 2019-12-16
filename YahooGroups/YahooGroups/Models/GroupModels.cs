using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YahooGroups.Models;
using System.Data.Entity;
using System.Web.Mvc;

namespace YahooGroups.Models
{
    public class GroupModels
    {
        [Key]
        public int groupId { get; set; }
        [Required]
        public string groupName { get; set; }
        public string groupDescripiton { get; set; }
        [Required]
        public int moderatorId { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUser { get; set; }
        public CategoryModel Category { get; set; }
        public bool privateGroup { get; set; }
    }
}