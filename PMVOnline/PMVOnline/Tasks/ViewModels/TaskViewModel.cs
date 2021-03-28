using PMVOnline.Accounts.Models;
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
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<UserModel> Users { get; set; }
        public List<UserModel> SelectedUsers { get; set; }


        public List<TaskActionModel> Tasks { get; set; }
        public bool IsLoading { get; set; }

        readonly INavigationService navigationService;
        readonly ITaskService taskService;
        readonly IApplicationSettings applicationServices;
        readonly IDateTimeService dateTimeService;

        public TaskViewModel(
            INavigationService navigationService,
            ITaskService taskService,
            IApplicationSettings applicationServices,
            IDateTimeService dateTimeService
            )
        {
            this.navigationService = navigationService;
            this.taskService = taskService;
            this.applicationServices = applicationServices;
            this.dateTimeService = dateTimeService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(NavigationKey.Reload))
            {
                LoadData();
            }
            return;
        }

        public override void RaiseIsActiveChanged()
        {
            base.RaiseIsActiveChanged();
            if (!IsActive || Tasks?.Count > 0)
            {
                return;
            }
            LoadData();
        }

        async Task LoadData()
        {
            IsLoading = true;
            await taskService.GetMyTasksAsync(0, 100).ContinueWith(t =>
            {
                if (t.Result != null)
                {
                    Tasks = new List<TaskActionModel>(t.Result);
                }
            });

            await taskService.GetUsersInMyTasksAsync().ContinueWith(t => Users = new List<UserModel>(t.Result));
            IsLoading = false;
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

            if (user.Departments?.Any(c => c.Name == DepartmentName.Director) == true && task.Status == Models.TaskStatus.Requested)
            {
                var result = await navigationService.NavigateAsync(Routes.ModerateTask, new NavigationParameters { { NavigationKey.TaskId, task.Id } });
                return;
            }

            var xx = await navigationService.NavigateAsync(Routes.TaskDetail, new NavigationParameters { { NavigationKey.TaskId, task.Id } });
        }

        ICommand _ReloadCommand;
        public ICommand ReloadCommand => _ReloadCommand = _ReloadCommand ?? new AsyncCommand(ExecuteReloadCommand);
        async Task ExecuteReloadCommand()
        {
            await LoadData();
        }

        ICommand _FilterCommand;
        public ICommand FilterCommand => _FilterCommand = _FilterCommand ?? new AsyncCommand(ExecuteFilterCommand);
        async Task ExecuteFilterCommand()
        {
        }

        ICommand _ChooseStartDateCommand;
        public ICommand ChooseStartDateCommand => _ChooseStartDateCommand = _ChooseStartDateCommand ?? new AsyncCommand(ExecuteChooseStartDateCommand);
        async Task ExecuteChooseStartDateCommand()
        {
            StartDate = await dateTimeService.PickDateAsync(current: StartDate, max: EndDate);
        }

        ICommand _ChooseEndDateCommand;
        public ICommand ChooseEndDateCommand => _ChooseEndDateCommand = _ChooseEndDateCommand ?? new AsyncCommand(ExecuteChooseEndDateCommand);
        async Task ExecuteChooseEndDateCommand()
        {
            EndDate = await dateTimeService.PickDateAsync(current: EndDate, min: StartDate);
        }

        ICommand _ChooseUserCommand;
        public ICommand ChooseUserCommand => _ChooseUserCommand = _ChooseUserCommand ?? new AsyncCommand(ExecuteChooseUserCommand);
        async Task ExecuteChooseUserCommand()
        {
        }
         
        ICommand _RemoveFilterCommand;
        public ICommand RemoveFilterCommand => _RemoveFilterCommand = _RemoveFilterCommand ?? new AsyncCommand(ExecuteRemoveFilterCommand);
        async Task ExecuteRemoveFilterCommand()
        {
            StartDate = null;
            EndDate = null;
            SelectedUsers = new List<UserModel>();
        } 
    }
}
