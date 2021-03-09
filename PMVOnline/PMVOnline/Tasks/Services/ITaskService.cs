using PMVOnline.Accounts.Models;
using PMVOnline.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PMVOnline.Tasks.Services
{
    public interface ITaskService
    {
        Task<UserModel> GetAssigneeAsync(TaskTarget target);
        Task<bool> CreateTaskAsync(CreateTaskModel task);
        Task<TaskModel[]> GetMyLastTasksAsync();
    }

    public class TaskService : ITaskService
    {
        public Task<bool> CreateTaskAsync(CreateTaskModel task)
        {
            return Task.FromResult(true);
        }

        public Task<UserModel> GetAssigneeAsync(TaskTarget target)
        {
            return Task.FromResult(new UserModel { Name = "Do Thanh" });
        }

        public Task<TaskModel[]> GetMyLastTasksAsync()
        {
            var myTasks = new List<TaskModel>
            {
                new TaskModel{ Assignee = "Do THanh Binh",  DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.High},
                new TaskModel{ Assignee = "Do THanh Binh",  DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.Normal},
                new TaskModel{ Assignee = "Do THanh Binh",  DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.Highest},
            };
            return Task.FromResult(myTasks.ToArray());
        }
    }
}
