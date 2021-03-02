using PMVOnline.Common.Bases;
using PMVOnline.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Homes.Models
{ 
    public class TaskActionModel : ModelBase
    {
        public TaskModel Task { get; set; }
        public string Action { get; set; }
        public string User { get; set; }
    }
}
