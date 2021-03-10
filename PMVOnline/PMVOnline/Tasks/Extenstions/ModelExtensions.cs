using PMVOnline.Api.Dtos.Tasks;
using PMVOnline.Homes.Models;
using PMVOnline.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Tasks.Extenstions
{
    public static class ModelExtensions
    {
        public static TaskActionModel ToModel(this TaskDto obj, Guid myId)
        {
            if (obj == null)
            {
                return null;
            }

            return new TaskActionModel
            {
                Task = new TaskModel
                {
                    Id = obj.TaskId,
                    DueDate = obj.DueDate,
                    Priority = (TaskPriority)obj.Priority,
                    Status = (TaskStatus)obj.Status,
                    Title = obj.Title
                },
                User = $"{obj.LastAction?.Actor?.Surname} {obj.LastAction?.Actor?.Name}",
                Action = obj.LastAction.ToModel(myId, obj.Assignee)
            };
        }

        public static string ToModel(this LastTaskHistoryDto dto, Guid myId, Guid assignee)
        {
            if (dto == null)
            {
                return string.Empty;
            }

            bool isMe = myId == dto.ActorId;
            bool isAssignee = myId == assignee;
            switch (dto.Action)
            {
                case ActionType.ApprovedTask:
                    return "đã duyệt sự vụ";
                case ActionType.ChangeAssignee:
                    return "thay đổi người phụ trách";
                case ActionType.Comment:
                    return "bình luận";
                case ActionType.CompletedTask:
                    return "hoàn thành sự vụ";
                case ActionType.CreateTask:
                    if (isMe && isAssignee || isMe)
                    {
                        return "tạo sự vụ";
                    }

                    if (isAssignee)
                    {
                        return "yêu cầu giải quyết sự vụ";
                    }
                    return "tạo sự vụ";
                case ActionType.Follow:
                    return "theo dõi sự vụ";
                case ActionType.IncompletedTask:
                    return "không hoàn thành sự vụ";
                case ActionType.RejectedTask:
                    return "không duyệt sự vụ";
                case ActionType.Reopen:
                    return "mở lại sự vụ";
                case ActionType.Unfollow:
                    return "bỏ theo dõi sự vụ";
                default:
                    return string.Empty;
            }
        }
    }
}
