using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Api.Dtos.Tasks
{
    public class TaskDto
    {
        public long TaskId { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public Priority Priority { get; set; }
        public Target Target { get; set; }
        public Status Status { get; set; }
        public Guid Assignee { get; set; }
        public LastTaskHistoryDto LastAction { get; set; }
    }
    public class LastTaskHistoryDto
    {
        public Guid ActorId { get; set; }
        public SimpleUserDto Actor { get; set; }
        public ActionType Action { get; set; }

    }

    public class SimpleUserDto  
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public enum Target
    {
        BuyCommodity, Payment, Storage, Make, Other, BuyOther
    }

    public enum Priority
    {
        Normal, High, Highest
    }

    public enum Status
    {
        Pending, Approved, Rejected, Completed, Incompleted
    }

    public enum ActionType
    {
        CreateTask, ApprovedTask, RejectedTask, Comment, CompletedTask, IncompletedTask, ChangeAssignee, Reopen, Follow, Unfollow
    }
}
