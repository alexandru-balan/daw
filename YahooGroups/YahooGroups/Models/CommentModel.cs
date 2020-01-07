using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YahooGroups.Models
{
    public class CommentModel
    {
        [Key]
        public int CommentId { get; set; }

        [Required(ErrorMessage = "You can't post an empty comment!")]
        public string Content { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}