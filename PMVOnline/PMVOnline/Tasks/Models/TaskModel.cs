using PMVOnline.Common.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Tasks.Models
{
    public class TaskBaseModel : ModelBase
    {
        public long Id { get; set; }
        public string Assignee { get; set; }
        public string Action { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
    }
    public class TaskModel : TaskBaseModel
    {
        public TaskStatus Status { get; set; }
    }
}