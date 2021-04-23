using PMVOnline.Accounts.Models;
using PMVOnline.Api;
using PMVOnline.Api.Dtos.Tasks;
using PMVOnline.Common.Services;
using PMVOnline.Homes.Models;
using PMVOnline.Tasks.Extenstions;
using PMVOnline.Tasks.Models;
using PMVOnline.Tasks.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMVOnline.Tasks.Services
{
    public interface ITaskService
    {
        Task<UserModel> GetAssigneeAsync(int target);
        Task<UserModel[]> GetAllUsersAsync(int target);
        Task<bool> CreateTaskAsync(TaskModel task);
        Task<bool> UpdateTaskAsync(TaskModel task);
        Task<TaskModel[]> GetMyLastTasksAsync();
        Task<TaskActionModel[]> GetMyTasksAsync(Guid[] users = null, DateTime? startDate = null, DateTime? endDate = null, int skipCount = 0, int maxResultCount = 100);
        Task<TaskActionModel[]> GetMyActionsAsync();
        Task<TaskModel> GetTaskAsync(long id);
        Task<TaskModel[]> GetReferenceTasksAsync(long id);
        Task<CommentModel[]> GetTaskCommentsAsync(long id);
        Task<FileModel[]> GetTaskFilesAsync(long id);
        Task<bool> ApproveTaskAsync(long id, bool approved, string note);
        Task<bool> FollowTaskAsync(long id, bool follow);
        Task<bool> ReopenAsync(long id);
        Task<bool> CompleteTaskAsync(long id, bool completed, DateTime completedDate, string note);
        Task<HistoryModel[]> GetTaskHistoryAsync(long id);
        Task<string> GetNoteAsync(long id);
        Task<bool> SendCommentAsync(long id, string comment, Guid[] files);
        Task<bool> RequestAsync(long id);
        Task<UserModel[]> GetUsersInMyTasksAsync();

        Task<TargetModel[]> GetAllTargetsAsync();
        Task<bool> RateAsync(long taskId, int rating);
    }

    public class TaskService : AuthApiProvider<AppApi>, ITaskService
    {
        readonly IApplicationSettings applicationSettings;

        public TaskService(IApplicationSettings applicationSettings)
        {
            this.applicationSettings = applicationSettings;
        }

        public async Task<bool> ApproveTaskAsync(long id, bool approved, string note)
        {
            var result = await Api.ApproveTask(new ApproveTaskRequestDto
            {
                Id = id,
                Approved = approved,
                Note = note
            });
            return result.Content;
        }

        public async Task<bool> CompleteTaskAsync(long id, bool completed, DateTime completedDate, string note)
        {
            var result = await Api.FinishTask(new FinishTaskRequestDto
            {
                Id = id,
                Completed = completed,
                Note = note,
                CompletedDate = completedDate
            });
            return result.Content;
        }

        public async Task<bool> CreateTaskAsync(TaskModel task)
        {
            var result = await Api.CreateTask(new CreateTaskRequestDto
            {
                AssigneeId = task.AssigneeId,
                Content = task.Content,
                DueDate = task.DueDate.ToUniversalTime(),
                Files = task.Files,
                Priority = (Priority)task.Priority,
                ReferenceTasks = task.ReferenceTasks?.Select(d => d.Id)?.ToArray() ?? new long[0],
                TargetId = task.Target.Id,
                Title = task.Title
            });
            return result.Content != null;
        }

        public async Task<bool> FollowTaskAsync(long id, bool follow)
        {
            var result = await Api.FollowTask(new FollowTaskRequestDto
            {
                Id = id,
                Follow = follow
            });
            return result.Content;
        }

        public async Task<UserModel> GetAssigneeAsync(int target)
        {
            var result = await Api.GetAssignee(target);
            return result.Content.ToModel();
        }

        public async Task<string> GetNoteAsync(long id)
        {
            var result = await Api.GetNote(id);
            return result.Content;
        }

        public async Task<TaskActionModel[]> GetMyActionsAsync()
        {
            var result = await Api.GetMyActions();
            if (result.Content != null)
            {
                return result.Content.Select(d => d.ToModel(applicationSettings.User.Id)).ToArray();
            }

            return new TaskActionModel[0];
        }

        public async Task<TaskModel[]> GetMyLastTasksAsync()
        {
            return new TaskModel[0];
        }

        public async Task<TaskActionModel[]> GetMyTasksAsync(Guid[] users = null, DateTime? startDate = null, DateTime? endDate = null, int skipCount = 0, int maxResultCount = 100)
        {
            if (startDate.HasValue)
            {
                startDate = startDate.Value.ToUniversalTime();
            }
            if (endDate.HasValue)
            {
                endDate = endDate.Value.ToUniversalTime();
            }
            var result = await Api.SearchMyTasks(new GetMyTaskRequestDto { SkipCount = skipCount, MaxResultCount = maxResultCount, Users = users ?? new Guid[0], StartDate = startDate, EndDate = endDate });
            if (result.Content != null)
            {
                return result.Content.Select(d => d.ToModel(applicationSettings.User.Id)).ToArray();
            }

            return new TaskActionModel[0];
        }

        public async Task<TaskModel> GetTaskAsync(long id)
        {
            var result = await Api.GetTask(id);
            return result.Content?.ToModel();
        }

        public async Task<CommentModel[]> GetTaskCommentsAsync(long id)
        {
            var result = await Api.GetTaskComments(id);
            if (result.Content != null)
            {
                return result.Content.Select(d => d.ToModel()).ToArray();
            }

            return new CommentModel[0];
        }

        public async Task<FileModel[]> GetTaskFilesAsync(long id)
        {
            var result = await Api.GetTaskFiles(id);
            if (result.Content != null)
            {
                return result.Content.Select(d => d.ToModel()).ToArray();
            }

            return new FileModel[0];
        }

        public async Task<HistoryModel[]> GetTaskHistoryAsync(long id)
        {
            var result = await Api.GetHistory(id);
            if (result.Content != null)
            {
                return result.Content.Select(d => d.ToModel()).ToArray();
            }

            return new HistoryModel[0];
        }

        public async Task<bool> ReopenAsync(long id)
        {
            var result = await Api.ReopenTask(new ReopenTaskRequestDto
            {
                Id = id
            });
            return result.Content;
        }

        public async Task<bool> SendCommentAsync(long id, string comment, Guid[] files)
        {
            var result = await Api.SendComment(id, new CommentRequestDto
            {
                Comment = comment,
                Files = files
            });
            return result.Content;
        }

        public async Task<bool> RequestAsync(long id)
        {
            var result = await Api.RequestTask(new ReopenTaskRequestDto
            {
                Id = id
            });
            return result.Content;
        }

        public async Task<UserModel[]> GetUsersInMyTasksAsync()
        {
            var result = await Api.GetUsersInMyTasks();
            if (result?.Content?.Length > 0)
            {
                return result.Content.Select(d => d.ToModel()).ToArray();
            }
            return new UserModel[0];
        }

        public async Task<TaskModel[]> GetReferenceTasksAsync(long id)
        {
            var result = await Api.GetReferenceTasks(id);
            if (result.Content?.Length > 0)
            {
                return result.Content.Select(d => d.ToModel()).ToArray();
            }
            return new TaskModel[0];
        }

        public async Task<bool> UpdateTaskAsync(TaskModel task)
        {
            var result = await Api.UpdateTask(new UpdateTaskRequestDto
            {
                Id = task.Id,
                AssigneeId = task.AssigneeId,
                Content = task.Content,
                DueDate = task.DueDate.ToUniversalTime(),
                Files = task.Files,
                Priority = (Priority)task.Priority,
                ReferenceTasks = task.ReferenceTasks?.Select(d => d.Id)?.ToArray() ?? new long[0],
                TargetId = task.Target.Id,
                Title = task.Title
            });
            return result.Content != null;
        }

        public async Task<UserModel[]> GetAllUsersAsync(int target)
        {
            var result = await Api.GetMembers((int)target);
            if (result?.Content?.Length > 0)
                return result.Content.Select(d => d.ToModel()).ToArray();
            return new UserModel[0];
        }

        public async Task<TargetModel[]> GetAllTargetsAsync()
        {
            var result = await Api.GetAllTargets();
            if (result?.Content?.Length > 0)
                return result.Content.Select(d => d.ToModel()).ToArray();
            return new TargetModel[0];
        }

        public async Task<bool> RateAsync(long taskId, int rating)
        {
            var result = await Api.Rate(taskId, new RatingRequestDto { Rating = rating });
            return result?.Content == true;
        }
    }
}
