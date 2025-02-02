﻿using PMVOnline.Accounts.Models;
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

namespace PMVOnline.Homes.ViewModels
{
    public class HomeViewModel : TabViewModelBase
    {
        public UserModel User { get; set; }
        public bool IsLoading { get; set; }
        public List<TaskActionModel> Actions { get; set; }

        readonly INavigationService navigationService;
        readonly ITaskService taskService;
        readonly IApplicationSettings applicationServices;

        public HomeViewModel(INavigationService navigationService, ITaskService taskService, IApplicationSettings applicationServices)
        {
            this.navigationService = navigationService;
            this.taskService = taskService;
            this.applicationServices = applicationServices;
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
            if (!IsActive || Actions?.Count > 0)
            {
                return;
            }
            User = applicationServices.User;
            LoadData();
        }

        async Task LoadData()
        {
            IsLoading = true;
            await taskService.GetMyActionsAsync().ContinueWith(d => Actions = new List<TaskActionModel>(d.Result));
            IsLoading = false;
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

            if (user.Departments?.Any(c => c.Name == DepartmentName.Director) == true && task.Status == Tasks.Models.TaskStatus.Requested)
            {
                var result = await navigationService.NavigateAsync(Routes.ModerateTask, new NavigationParameters { { NavigationKey.TaskId, task.Id } });
                return;
            } 
            var xx = await navigationService.NavigateAsync(task.Status < Tasks.Models.TaskStatus.Rated ? Routes.TaskDetail : Routes.TaskRating, new NavigationParameters { { NavigationKey.TaskId, task.Id } });
        }



        ICommand _ReloadCommand;
        public ICommand ReloadCommand => _ReloadCommand = _ReloadCommand ?? new AsyncCommand(ExecuteReloadCommand);
        async Task ExecuteReloadCommand()
        {
            await LoadData();
        }

    }
}
