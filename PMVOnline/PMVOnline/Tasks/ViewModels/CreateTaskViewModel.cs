using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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
        public CreateTaskModel Task { get; set; } = new CreateTaskModel();
        public TaskModel TaskCloned { get; set; }
        public ObservableCollection<FileModel> Files { get; set; }

        readonly INavigationService navigationService;
        readonly IDialogService dialogService;
        readonly IDateTimeService dateTimeService;
        readonly ITaskService taskService;
        private readonly IFileService fileService;

        List<TaskModel> myTasks;
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

        public override async Task InitializeAsync(INavigationParameters parameters)
        {
            Files = new ObservableCollection<FileModel>();
            myTasks = new List<TaskModel>(await taskService.GetMyTasksAsync());
        }

        ICommand _ChooseTargetCommand;
        public ICommand ChooseTargetCommand => _ChooseTargetCommand = _ChooseTargetCommand ?? new AsyncCommand(ExecuteChooseTargetCommand);
        async Task ExecuteChooseTargetCommand()
        {
            var result = await dialogService.ShowDialogAsync(DialogRoutes.ChooseTarget, Task.Target != null ? new DialogParameters { { NavigationKey.Target, Task.Target } } : new DialogParameters { });
            if (result?.Parameters?.ContainsKey(NavigationKey.Target) == true)
            {
                Task.Target = result.Parameters.GetValue<TargetModel>(NavigationKey.Target);
                IsBusy = true;
                var assignee = await taskService.GetAssigneeAsync(Task.Target.Target);
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
            var param = await dialogService.ShowDialogAsync(DialogRoutes.MultiSelectTask, new DialogParameters { { NavigationKey.ReferenceTasks, Task.ReferenceTasks }, { NavigationKey.MyTasks, myTasks } });
            if (param?.Parameters?.ContainsKey(NavigationKey.ReferenceTasks) == true)
            {
                Task.ReferenceTasks = param.Parameters.GetValue<List<TaskModel>>(NavigationKey.ReferenceTasks).Select(d => d.Id).ToArray();
            }
        }

        ICommand _CloneCommand;
        public ICommand CloneCommand => _CloneCommand = _CloneCommand ?? new AsyncCommand(ExecuteCloneCommand);
        async Task ExecuteCloneCommand()
        {
            var param = await dialogService.ShowDialogAsync(DialogRoutes.SelectTask, new DialogParameters { { NavigationKey.CloneTask, TaskCloned }, { NavigationKey.MyTasks, myTasks } });
            if (param?.Parameters?.ContainsKey(NavigationKey.CloneTask) == true)
            {
                TaskCloned = param.Parameters.GetValue<TaskModel>(NavigationKey.CloneTask);
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

        async Task UploadFiles()
        {
            List<Task<FileModel>> files = new List<Task<FileModel>>();
            for (int i = 0; i < Files.Count; i++)
            {
                var file = Files[i];
                var stream = await (new FileResult(file.FullPath)).OpenReadAsync();
                files.Add(fileService.UploadAsync(stream, file.FileName));
            }

            await System.Threading.Tasks.Task.WhenAll(files).ContinueWith(t => Task.Files = t.Result?.Select(c => c.Id)?.ToArray());
        }
    }
}