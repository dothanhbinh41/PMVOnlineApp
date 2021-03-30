using PMVOnline.Api.Dtos.Tasks;
using PMVOnline.Common.Bases;
using PMVOnline.Tasks.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Tasks.Models
{
    public class TaskModel : ModelBase
    {
        public long Id { get; set; }
        public string Assignee { get; set; }
        public Guid AssigneeId { get; set; }
        public Guid CreatorId { get; set; }
        public string Creator { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public ActionType LastAction { get; set; }

        public DateTime? Date { get; set; }
        public TargetModel Target { get; set; }
        public long[] ReferenceTasks { get; set; }
        public Guid[] Files { get; set; }
        public string Content { get; set; }

        public void OnDueDateChanged()
        {
            Date = DueDate;
        }
    } 
}