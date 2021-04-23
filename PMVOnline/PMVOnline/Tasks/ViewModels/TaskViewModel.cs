using PMVOnline.Accounts.Models;
using PMVOnline.Common.Bases;
using PMVOnline.Common.Services;
using PMVOnline.Homes.Models;
using PMVOnline.Tasks.Models;
using PMVOnline.Tasks.Services;
using Prism.Navigation;
using Prism.Services.Dialogs;
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
        private readonly IDialogService dialogService;

        public TaskViewModel(
            INavigationService navigationService,
            ITaskService taskService,
            IApplicationSettings applicationServices,
            IDateTimeService dateTimeService,
            IDialogService dialogService
            )
        {
            this.navigationService = navigationService;
            this.taskService = taskService;
            this.applicationServices = applicationServices;
            this.dateTimeService = dateTimeService;
            this.dialogService = dialogService;
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
            await LoadTasks();
            await LoadUsers();
            IsLoading = false;
        }

        async Task LoadTasks()
        {
            var result = await taskService.GetMyTasksAsync(SelectedUsers?.Select(d => d.Id)?.ToArray() ?? new Guid[0], StartDate, EndDate, 0, 100);
            if (result != null)
            {
                Tasks = new List<TaskActionModel>(result);
            }
        }

        async Task LoadUsers()
        {
            var result = await taskService.GetUsersInMyTasksAsync();
            if (result?.Length > 0)
            {
                Users = new List<UserModel>(result);
            }
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

            var xx = await navigationService.NavigateAsync(task.Status < Models.TaskStatus.Rated ? Routes.TaskDetail : Routes.TaskRating, new NavigationParameters { { NavigationKey.TaskId, task.Id } });
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
            var startDate = await dateTimeService.PickDateAsync(current: StartDate, max: EndDate);
            if (StartDate == startDate)
            {
                return;
            }
            StartDate = startDate;
            IsBusy = true;
            await LoadTasks();
            IsBusy = false;
        }

        ICommand _ChooseEndDateCommand;
        public ICommand ChooseEndDateCommand => _ChooseEndDateCommand = _ChooseEndDateCommand ?? new AsyncCommand(ExecuteChooseEndDateCommand);
        async Task ExecuteChooseEndDateCommand()
        {
            var endDate = await dateTimeService.PickDateAsync(current: EndDate, min: StartDate);
            if (EndDate == endDate)
            {
                return;
            }
            EndDate = endDate;
            IsBusy = true;
            await LoadTasks();
            IsBusy = false;
        }

        ICommand _ChooseUserCommand;
        public ICommand ChooseUserCommand => _ChooseUserCommand = _ChooseUserCommand ?? new AsyncCommand(ExecuteChooseUserCommand);
        async Task ExecuteChooseUserCommand()
        {
            var result = await dialogService.ShowDialogAsync(DialogRoutes.ChooseUsers, new DialogParameters { { NavigationKey.SelectedUsers, SelectedUsers }, { NavigationKey.Users, Users } });
            if (!result.Parameters.ContainsKey(NavigationKey.SelectedUsers))
            {
                return;
            }

            SelectedUsers = result.Parameters.GetValue<List<UserModel>>(NavigationKey.SelectedUsers);
            IsBusy = true;
            await LoadTasks();
            IsBusy = false;
        }

        ICommand _RemoveFilterCommand;
        public ICommand RemoveFilterCommand => _RemoveFilterCommand = _RemoveFilterCommand ?? new AsyncCommand(ExecuteRemoveFilterCommand);
        async Task ExecuteRemoveFilterCommand()
        {
            StartDate = null;
            EndDate = null;
            SelectedUsers = new List<UserModel>();
            IsBusy = true;
            await LoadTasks();
            IsBusy = false;
        }
    }
}
