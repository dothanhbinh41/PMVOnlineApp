﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using PMVOnline.Common.Bases;
using PMVOnline.Common.Services;
using PMVOnline.Tasks.Models;
using Prism.Navigation;
using Prism.Services.Dialogs;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PMVOnline.Tasks.ViewModels
{
    public class CreateTaskViewModel : ViewModelBase
    {

        public CreateTaskModel Task { get; set; } = new CreateTaskModel();

        readonly INavigationService navigationService;
        readonly IDialogService dialogService;
        readonly IDateTimeService dateTimeService;

        public CreateTaskViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDateTimeService dateTimeService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.dateTimeService = dateTimeService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.GetNavigationMode() == NavigationMode.Back && parameters.ContainsKey(NavigationKey.ReferenceTasks))
            {
                var t = parameters.GetValue<List<TaskBaseModel>>(NavigationKey.ReferenceTasks);
                Task.ReferenceTasks = t.Select(d => d.Id).ToArray();
            }
        }


        ICommand _ChooseTargetCommand;
        public ICommand ChooseTargetCommand => _ChooseTargetCommand = _ChooseTargetCommand ?? new AsyncCommand(ExecuteChooseTargetCommand);
        async Task ExecuteChooseTargetCommand()
        {
            var result = await dialogService.ShowDialogAsync(DialogRoutes.ChooseTarget, Task.Target != null ? new DialogParameters { { NavigationKey.Target, Task.Target } } : new DialogParameters { });
            if (result?.Parameters?.ContainsKey(NavigationKey.Target) == true)
            {
                Task.Target = result.Parameters.GetValue<TargetModel>(NavigationKey.Target);
            }
        }

        ICommand _ChoosePriorityCommand;
        public ICommand ChoosePriorityCommand => _ChoosePriorityCommand = _ChoosePriorityCommand ?? new AsyncCommand(ExecuteChoosePriorityCommand);
        async Task ExecuteChoosePriorityCommand()
        {
            var result = await dialogService.ShowDialogAsync(DialogRoutes.ChoosePriority, new DialogParameters { { NavigationKey.Priority, Task.Priority } });
            if (result?.Parameters?.ContainsKey(NavigationKey.Priority) == true)
            {
                Task.Priority = result.Parameters.GetValue<TaskPriority>(NavigationKey.Priority);
            }
        }

        ICommand _ChooseDueDateCommand;
        public ICommand ChooseDueDateCommand => _ChooseDueDateCommand = _ChooseDueDateCommand ?? new AsyncCommand(ExecuteChooseDueDateCommand);
        async Task ExecuteChooseDueDateCommand()
        {
            var date = await dateTimeService.PickDateTimeAsync(DateTime.Now, Task.Date);
            if (date.HasValue)
            {
                Task.Date = date.Value;
            }
        }

        ICommand _ReferenceTasksCommand;
        public ICommand ReferenceTasksCommand => _ReferenceTasksCommand = _ReferenceTasksCommand ?? new AsyncCommand(ExecuteReferenceTasksCommand);
        async Task ExecuteReferenceTasksCommand()
        {
            await navigationService.NavigateAsync(Routes.TaskReference);
        }
    }
}