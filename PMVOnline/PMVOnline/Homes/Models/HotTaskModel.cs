using PMVOnline.Common.Bases;
using PMVOnline.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Homes.Models
{ 
    public class TaskActionModel : TaskModel
    { 
        public string Action { get; set; }
        public string Actor { get; set; }
        public bool IsCreatedByMe { get; set; }
    }
}
