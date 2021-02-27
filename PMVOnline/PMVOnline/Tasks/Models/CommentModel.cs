using PMVOnline.Common.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Tasks.Models
{
    public class CommentModel : ModelBase
    {
        public string Sender { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public FileModel[] Files { get; set; }
    }
}
