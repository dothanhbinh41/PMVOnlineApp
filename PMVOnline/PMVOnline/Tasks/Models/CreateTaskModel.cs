using PMVOnline.Tasks.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Tasks.Models
{
    public class CreateTaskModel : TaskModel
    {
        public DateTime? Date { get; set; }
        public TargetModel Target { get; set; }
        public long[] ReferenceTasks { get; set; }
        public string Content { get; set; }
    }
}
