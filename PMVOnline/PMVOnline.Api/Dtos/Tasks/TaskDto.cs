using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Api.Dtos.Tasks
{
    public class FullTaskDto 
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public Priority Priority { get; set; }
        public Target Target { get; set; }
        public Status Status { get; set; }
        public ActionType LastAction { get; set; }
        public Guid AssigneeId { get; set; } 
        public Guid CreatorId { get; set; } 
        //public TaskFileDto[] TaskFiles { get; set; }
        public ReferenceTaskDto[] ReferenceTasks { get; set; } 
        public virtual SimpleUserDto Assignee { get; set; }
    }

    public class ReferenceTaskDto
    { 
        public long ReferenceTaskId { get; set; }
    }

    public class TaskFileDto
    {
        public Guid FileId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }

    public class FileDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }

    public class TaskDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public Priority Priority { get; set; }
        public Target Target { get; set; }
        public Status Status { get; set; }
        public SimpleUserDto Assignee { get; set; }
        public SimpleUserDto Creator { get; set; }
        public SimpleUserDto LastModifier { get; set; }
        public ActionType LastAction { get; set; }
    }

    public class CreateTaskResultDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public Priority Priority { get; set; }
        public Target Target { get; set; }
        public Status Status { get; set; } 
    }

    public class CreateTaskRequestDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Target Target { get; set; }
        public Priority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public Guid[] Files { get; set; }
        public long[] ReferenceTasks { get; set; }
        public Guid AssigneeId { get; set; }
    }

    public class TaskCommentDto
    {
        public Guid Id { get; set; }
        public long TaskId { get; set; }
        public string Comment { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreationTime { get; set; }
        public TaskFileDto[] FileIds { get; set; }
        public SimpleUserDto User { get; set; }
    }

    public class TaskHistoryDto
    { 
        public SimpleUserDto Actor { get; set; }
        public ActionType Action { get; set; }
        public string Note { get; set; }
        public DateTime CreationTime { get; set; }

    }

    public class ApproveTaskRequestDto
    {
        public long Id { get; set; }
        public bool Approved { get; set; }
        public string Note { get; set; }
    }

    public class FinishTaskRequestDto
    {
        public long Id { get; set; }
        public bool Completed { get; set; }
        public DateTime CompletedDate { get; set; }
        public string Note { get; set; }
    }
    
    public class FollowTaskRequestDto
    {
        public long Id { get; set; }
        public bool Follow { get; set; } 
    } 
    
    public class ReopenTaskRequestDto
    {
        public long Id { get; set; } 
    }

    public class SimpleUserDto  
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class CommentRequestDto
    {
        public string  Comment { get; set; }
        public Guid[]  Files { get; set; }
    }

    public class GuideResultDto
    {
        public string Content { get; set; }
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
