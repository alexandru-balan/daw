using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YahooGroups.Models
{
    public class FileModel
    {
        [Key]
        public int FileId { get; set; }

        public string FileName { get; set; }

        public byte[] Content { get; set; }

        public FileType FileType { get; set; }

        public string UserId { get; set; }

        public int GroupID { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}