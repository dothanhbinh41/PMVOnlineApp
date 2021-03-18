using PMVOnline.Common.Bases;
using PMVOnline.Common.Services;
using PMVOnline.Homes.Models;
using PMVOnline.Tasks.Models;
using PMVOnline.Tasks.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PMVOnline.Tasks.ViewModels
{
    public class TaskViewModel : TabViewModelBase
    {
        public List<TaskActionModel> Tasks { get; set; }


        readonly INavigationService navigationService;
        private readonly ITaskService taskService;
        private readonly IApplicationSettings applicationServices;

        public TaskViewModel(INavigationService navigationService, ITaskService taskService, IApplicationSettings applicationServices)
        {
            this.navigationService = navigationService;
            this.taskService = taskService;
            this.applicationServices = applicationServices;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

        }

        public override void RaiseIsActiveChanged()
        {
            base.RaiseIsActiveChanged();
            taskService.GetMyTasksAsync(0).ContinueWith(t =>
            {
                if (t.Result != null)
                {
                    Tasks = new List<TaskActionModel>(t.Result);
                }
            });
        }

        ICommand _CreateCommand;
        public ICommand CreateCommand => _CreateCommand = _CreateCommand ?? new AsyncCommand(ExcuteCreateCommand);
        async Task ExcuteCreateCommand()
        {
            await navigationService.NavigateAsync(Routes.CreateTask);
        }

        ICommand _ViewDetailCommand;
        public ICommand ViewDetailCommand => _ViewDetailCommand = _ViewDetailCommand ?? new AsyncCommand<TaskActionModel>(ExecuteViewDetailCommand);
        async Task ExecuteViewDetailCommand(TaskActionModel task)
        {
            var user = applicationServices.User;

            if (user == null)
            {
                return;
            }

            if (user.Roles?.Any(c => c.Name == "admin") == true && task.Status == Models.TaskStatus.Pending)
            {
                var result = await navigationService.NavigateAsync(Routes.ModerateTask, new NavigationParameters { { NavigationKey.TaskId, task.Id } });
                return;
            }

            var xx = await navigationService.NavigateAsync(Routes.TaskDetail, new NavigationParameters { { NavigationKey.TaskId, task.Id } });
        }

    }
}
