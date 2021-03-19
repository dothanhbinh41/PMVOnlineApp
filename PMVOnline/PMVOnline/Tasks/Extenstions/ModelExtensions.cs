using PMVOnline.Accounts.Models;
using PMVOnline.Api.Dtos.Tasks;
using PMVOnline.Homes.Models;
using PMVOnline.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static HistoryModel ToModel(this TaskHistoryDto obj)
        {
            if (obj == null)
            {
                return null;
            }

            return new HistoryModel
            {
                Date = obj.CreationTime,
                Actor = $"{obj.Actor?.Surname} {obj.Actor?.Name}",
                Action = obj.Action.ToAction(),
                Note = obj.Note
            };
        }

        public static FileModel ToModel(this FileDto obj)
        {
            if (obj == null)
            {
                return null;
            }

            return new FileModel
            {
                Id = obj.Id,
                FileName = obj.Name,
                FullPath = obj.Path
            };
        }

        public static UserModel ToModel(this SimpleUserDto obj)
        {
            if (obj == null)
            {
                return null;
            }

            return new UserModel
            {
                Id = obj.Id,
                Name = obj.Name,
                Surname = obj.Surname
            };
        }

        public static CommentModel ToModel(this TaskCommentDto obj)
        {
            if (obj == null)
            {
                return null;
            }

            return new CommentModel
            {
                Date = obj.CreationTime,
                Sender = $"{obj.User?.Surname} {obj.User?.Name}",
                Content = obj.Comment,
                Files = obj.FileIds?.Select(d => new FileModel { Id = d.FileId })?.ToArray()
            };
        }

        public static TaskDetailModel ToModel(this FullTaskDto obj)
        {
            if (obj == null)
            {
                return null;
            }
            return new TaskDetailModel
            {
                Id = obj.Id,
                Content = obj.Content,
                Assignee = $"{obj.Assignee.Surname} {obj.Assignee.Name}",
                Title = obj.Title,
                DueDate = obj.DueDate,
                Status = (TaskStatus)obj.Status,
                Target = new ViewModels.TargetModel { Target = (TaskTarget)obj.Target },
                Priority = (TaskPriority)obj.Priority,
                ReferenceTasks = obj.ReferenceTasks?.Select(c => c.ReferenceTaskId).ToArray(),
                AssigneeId = obj.AssigneeId,
                CreatorId = obj.CreatorId,
                LastAction = obj.LastAction
            };
        }
    }
}
