using PMVOnline.Accounts.Models;
using PMVOnline.Api;
using PMVOnline.Common.Services;
using PMVOnline.Homes.Models;
using PMVOnline.Tasks.Extenstions;
using PMVOnline.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMVOnline.Tasks.Services
{
    public interface ITaskService
    {
        Task<UserModel> GetAssigneeAsync(TaskTarget target);
        Task<bool> CreateTaskAsync(CreateTaskModel task);
        Task<TaskModel[]> GetMyLastTasksAsync();
        Task<TaskActionModel[]> GetMyTasksAsync(int skip, int max = 20);
        Task<TaskActionModel[]> GetMyActionsAsync();
    }

    public class TaskService : AuthApiProvider<AppApi>, ITaskService
    {
        private readonly IApplicationSettings applicationSettings;

        public TaskService(IApplicationSettings applicationSettings)
        {
            this.applicationSettings = applicationSettings;
        }
        public Task<bool> CreateTaskAsync(CreateTaskModel task)
        {
            return Task.FromResult(true);
        }

        public Task<UserModel> GetAssigneeAsync(TaskTarget target)
        {
            return Task.FromResult(new UserModel { Name = "Do Thanh" });
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

        public async Task<TaskActionModel[]> GetMyTasksAsync(int skip, int max = 20)
        {
            var result = await Api.GetMyTasks(skip, max);
            if (result.Content != null)
            {
                return result.Content.Select(d => d.ToModel(applicationSettings.User.Id)).ToArray();
            }

            return new TaskActionModel[0];
        }
    }
}
