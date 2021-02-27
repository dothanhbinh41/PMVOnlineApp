using PMVOnline.Common.Bases;
using PMVOnline.Tasks.Models;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace PMVOnline.Tasks.ViewModels.Admins
{
    public class ModerateTaskViewModel : ViewModelBase
    {
        public CreateTaskModel Task { get; set; } = new CreateTaskModel();
        public List<FileModel> Files { get; set; }

        readonly INavigationService navigationService;
        readonly IDialogService dialogService;

        List<TaskBaseModel> myTasks;
        public ModerateTaskViewModel(
            INavigationService navigationService,
            IDialogService dialogService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            myTasks = new List<TaskBaseModel>
            {
                new TaskModel{ Assignee = "Do THanh Binh", Action = "Phe Duyet", DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.High},
                new TaskModel{ Assignee = "Do THanh Binh", Action = "Phe Duyet", DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.Normal},
                new TaskModel{ Assignee = "Do THanh Binh", Action = "Phe Duyet", DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.Highest},
            };
            Files = new List<FileModel>();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }


        ICommand _ReferenceTasksCommand;
        public ICommand ReferenceTasksCommand => _ReferenceTasksCommand = _ReferenceTasksCommand ?? new AsyncCommand(ExecuteReferenceTasksCommand);
        async Task ExecuteReferenceTasksCommand()
        {
            var param = await dialogService.ShowDialogAsync(DialogRoutes.MultiSelectTask, new DialogParameters { { NavigationKey.ReferenceTasks, Task.ReferenceTasks }, { NavigationKey.MyTasks, myTasks } });
            if (param?.Parameters?.ContainsKey(NavigationKey.ReferenceTasks) == true)
            {
                Task.ReferenceTasks = param.Parameters.GetValue<List<TaskBaseModel>>(NavigationKey.ReferenceTasks).Select(d => d.Id).ToArray();
            }
        }


        ICommand _CreateCommand;
        public ICommand CreateCommand => _CreateCommand = _CreateCommand ?? new AsyncCommand(ExecuteCreateCommand);
        async Task ExecuteCreateCommand()
        {
        }
    }
}
