using PMVOnline.Common.Bases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PMVOnline.Tasks.Models
{
    public class FileModel : ModelBase
    {
        public Guid Id { get; set; }
        public string FullPath { get; set; } 
        public string ContentType { get; set; } 
        public string FileName { get; set; }  
    }
}
