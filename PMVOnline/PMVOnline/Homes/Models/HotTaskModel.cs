using PMVOnline.Common.Bases;
using PMVOnline.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Homes.Models
{
    public class HotTaskModel : ModelBase
    {
        public long Id { get; set; }
        public string Assignee { get; set; }
        public string Action { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
