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
                IsCreatedByMe = obj.Creator.Id == myId,
                Id = obj.Id,
                DueDate = obj.DueDate,
                Priority = (TaskPriority)obj.Priority,
                Status = (TaskStatus)obj.Status,
                Title = obj.Title,
                Creator = $"{obj.Creator?.Surname} {obj.Creator?.Name}",
                Actor = $"{obj.LastModifier?.Surname} {obj.LastModifier?.Name}",
                Action = obj.LastAction.ToAction()
            };
        }

        public static string ToAction(this ActionType dto)
        { 
            switch (dto)
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
                    return "yêu cầu giải quyết sự vụ";
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
