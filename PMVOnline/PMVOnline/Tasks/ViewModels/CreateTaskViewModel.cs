using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using PMVOnline.Common.Bases;
using PMVOnline.Common.Services;
using PMVOnline.Tasks.Models;
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

        List<TaskModel> myTasks;
        public CreateTaskViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDateTimeService dateTimeService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.dateTimeService = dateTimeService;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            myTasks = new List<TaskModel>
            {
                new TaskModel{ Assignee = "Do THanh Binh",  DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.High},
                new TaskModel{ Assignee = "Do THanh Binh",  DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.Normal},
                new TaskModel{ Assignee = "Do THanh Binh",  DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.Highest},
            };
            Files = new ObservableCollection<FileModel>();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.GetNavigationMode() == Prism.Navigation.NavigationMode.Back && parameters.ContainsKey(NavigationKey.ReferenceTasks))
            {
                var t = parameters.GetValue<List<TaskModel>>(NavigationKey.ReferenceTasks); 
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
        } 
    }
}