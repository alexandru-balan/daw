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

        [Required(ErrorMessage = "You must provide a group name")]
        public string groupName { get; set; }

        public string groupDescripiton { get; set; }

        public string moderatorId { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

        [Required(ErrorMessage = "You must pick one category")]
        public int CategoryId { get; set; }

        public bool privateGroup { get; set; }

        public virtual ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();
        public virtual ICollection<FileModel> Files { get; set; } = new List<FileModel>();
    }
}