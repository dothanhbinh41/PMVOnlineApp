using PMVOnline.Common.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Tasks.Models
{
    public class TaskModel : ModelBase
    {
        public long Id { get; set; }
        public string Assignee { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
    }
     

    public class TaskDetailModel : CreateTaskModel
    { 
    } 
}