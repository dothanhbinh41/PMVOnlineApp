﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using PMVOnline.Accounts.Models;
using PMVOnline.Common.Bases;
using PMVOnline.Common.Services;
using PMVOnline.Tasks.Models;
using PMVOnline.Tasks.Services;
using Prism.Navigation;
using Prism.Services.Dialogs;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PMVOnline.Tasks.ViewModels
{
    public class CreateTaskViewModel : ViewModelBase
    {
        public TaskModel Task { get; set; } = new TaskModel();
        public TaskModel TaskCloned { get; set; } 
        public ObservableCollection<FileModel> Files { get; set; }

        readonly INavigationService navigationService;
        readonly IDialogService dialogService;
        readonly IDateTimeService dateTimeService;
        readonly ITaskService taskService;
        readonly IFileService fileService;

        List<TaskModel> myTasks;
        List<TargetModel> allTargets;
        UserModel[] users;
        UserModel assignee;

        public CreateTaskViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDateTimeService dateTimeService,
            ITaskService taskService,
            IFileService fileService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.dateTimeService = dateTimeService;
            this.taskService = taskService;
            this.fileService = fileService;
        }
         
        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            Files = new ObservableCollection<FileModel>();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await LoadData();
        }
         

        async Task LoadData()
        {
            myTasks = new List<TaskModel>(await taskService.GetMyTasksAsync());
            allTargets = new List<TargetModel>(await taskService.GetAllTargetsAsync());
        }

        ICommand _ChooseTargetCommand;
        public ICommand ChooseTargetCommand => _ChooseTargetCommand = _ChooseTargetCommand ?? new AsyncCommand(ExecuteChooseTargetCommand);
        async Task ExecuteChooseTargetCommand()
        {
            var result = await dialogService.ShowDialogAsync(DialogRoutes.ChooseTarget, Task.Target != null ? new DialogParameters { { NavigationKey.Target, Task.Target },{ NavigationKey.AllTargets, allTargets } } : new DialogParameters { { NavigationKey.AllTargets, allTargets } });
            if (result?.Parameters?.ContainsKey(NavigationKey.Target) == true)
            {
                Task.Target = result.Parameters.GetValue<TargetModel>(NavigationKey.Target);
                IsBusy = true;
                assignee = await taskService.GetAssigneeAsync(Task.Target.Id);
                users = await taskService.GetAllUsersAsync(Task.Target.Id);
                IsBusy = false;
                if (assignee == null)
                {
                    return;
                }
                Task.Assignee = assignee.FullName;
                Task.AssigneeId = assignee.Id;
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
                Task.DueDate = date.Value;
            }
        }

        ICommand _ReferenceTasksCommand;
        public ICommand ReferenceTasksCommand => _ReferenceTasksCommand = _ReferenceTasksCommand ?? new AsyncCommand(ExecuteReferenceTasksCommand);
        async Task ExecuteReferenceTasksCommand()
        {
            var myRef = Task.ReferenceTasks;
            var param = await dialogService.ShowDialogAsync(DialogRoutes.MultiSelectTask, new DialogParameters { { NavigationKey.ReferenceTasks, new List<TaskModel>(myRef ?? new TaskModel[0]) }, { NavigationKey.MyTasks, myTasks }, { NavigationKey.Editable, true } });
            if (param?.Parameters?.ContainsKey(NavigationKey.ReferenceTasks) == true)
            {
                Task.ReferenceTasks = param.Parameters.GetValue<List<TaskModel>>(NavigationKey.ReferenceTasks).ToArray();
            }
        }

        ICommand _CloneCommand;
        public ICommand CloneCommand => _CloneCommand = _CloneCommand ?? new AsyncCommand(ExecuteCloneCommand);
        async Task ExecuteCloneCommand()
        {
            var param = await dialogService.ShowDialogAsync(DialogRoutes.SelectTask, new DialogParameters { { NavigationKey.CloneTask, TaskCloned }, { NavigationKey.MyTasks, myTasks } });
            if (param?.Parameters?.ContainsKey(NavigationKey.CloneTask) == true)
            {
                IsBusy = true;
                TaskCloned = param.Parameters.GetValue<TaskModel>(NavigationKey.CloneTask);
                Task = await taskService.GetTaskAsync(TaskCloned.Id);
                if (Task.Target!=null)
                users = await taskService.GetAllUsersAsync(Task.Target.Id);
                assignee = new UserModel { Id = Task.AssigneeId, FullName = Task.Assignee };
                Files = new ObservableCollection<FileModel>(await taskService.GetTaskFilesAsync(TaskCloned.Id) ?? new FileModel[0]);
                Task.ReferenceTasks = await taskService.GetReferenceTasksAsync(TaskCloned.Id) ?? new TaskModel[0];
                IsBusy = false;
            }
        }

        ICommand _AddFileCommand;
        public ICommand AddFileCommand => _AddFileCommand = _AddFileCommand ?? new AsyncCommand(ExecuteAddFileCommand);
        async Task ExecuteAddFileCommand()
        {
            var files = await FilePicker.PickMultipleAsync(PickOptions.Default);
            if (files?.Any() == true)
            {
                foreach (var item in files)
                {
                    Files.Add(new FileModel { ContentType = item.ContentType, FileName = item.FileName, FullPath = item.FullPath });
                }
            }
        }

        ICommand _RemoveFileCommand;
        public ICommand RemoveFileCommand => _RemoveFileCommand = _RemoveFileCommand ?? new Command<FileModel>(ExecuteRemoveFileCommand);
        void ExecuteRemoveFileCommand(FileModel file)
        {
            Files?.Remove(file);
        }

        ICommand _CreateCommand;
        public ICommand CreateCommand => _CreateCommand = _CreateCommand ?? new AsyncCommand(ExecuteCreateCommand);
        async Task ExecuteCreateCommand()
        {
            IsBusy = true;
            await UploadFiles();
            var result = await taskService.CreateTaskAsync(Task);
            IsBusy = false;

            if (result)
            {
                Toast("Tao thanh cong");
                await navigationService.GoBackAsync(new NavigationParameters { { NavigationKey.Reload, true } });
            }
            else
            {
                Toast("Tao that bai");
            }
        }



        ICommand _ChooseAssigneeCommand;
        public ICommand ChooseAssigneeCommand => _ChooseAssigneeCommand = _ChooseAssigneeCommand ?? new AsyncCommand(ExecuteChooseAssigneeCommand);
        async Task ExecuteChooseAssigneeCommand()
        {
            if (users == null || assignee == null)
            {
                return;
            }

            var selected = await dialogService.ShowDialogAsync(DialogRoutes.ChooseUsers, new DialogParameters { { NavigationKey.Count, 1 }, { NavigationKey.SelectedUsers, new List<UserModel> { assignee } }, { NavigationKey.Users, new List<UserModel>(users) } });
            if (selected.Parameters.ContainsKey(NavigationKey.SelectedUsers))
            {
                assignee = selected.Parameters.GetValue<List<UserModel>>(NavigationKey.SelectedUsers).FirstOrDefault();
                Task.AssigneeId = assignee?.Id ?? Guid.Empty;
                Task.Assignee = assignee?.FullName;
            }
        }

        async Task UploadFiles()
        {
            List<Task<FileModel>> files = new List<Task<FileModel>>();
            var notUpload = Files?.Where(d => d.Id == Guid.Empty)?.ToList() ?? new List<FileModel>();
            var uploaded = Files?.Where(d => d.Id != Guid.Empty)?.ToArray() ?? new FileModel[0];
            for (int i = 0; i < notUpload.Count; i++)
            {
                var file = notUpload[i];
                var stream = await (new FileResult(file.FullPath)).OpenReadAsync();
                files.Add(fileService.UploadAsync(stream, file.FileName));
            }

            var result = await System.Threading.Tasks.Task.WhenAll(files);
            Task.Files = result?.Select(d => d.Id)?.Concat(uploaded.Select(d => d.Id))?.ToArray() ?? new Guid[0];
        }
    }
}