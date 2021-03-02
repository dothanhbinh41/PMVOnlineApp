using PMVOnline.Common.Bases;
using PMVOnline.Tasks.Models;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PMVOnline.Tasks.ViewModels
{
    public class TaskReferenceViewModel : ViewModelBase
    {
        public List<TaskModel> Tasks { set; get; }  
        readonly INavigationService navigationService;
        readonly IDialogService dialogService;
        List<TaskModel> myTasks;
        public TaskReferenceViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            Tasks = new List<TaskModel> { };
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            myTasks = new List<TaskModel>
            {
                new TaskModel{ Assignee = "Do THanh Binh",   DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.High},
                new TaskModel{ Assignee = "Do THanh Binh",  DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.Normal},
                new TaskModel{ Assignee = "Do THanh Binh",  DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.Highest},
            };
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            parameters.Add(NavigationKey.ReferenceTasks, Tasks);
        }

        ICommand _PickReferenceTaskCommand;
        public ICommand PickReferenceTaskCommand => _PickReferenceTaskCommand = _PickReferenceTaskCommand ?? new AsyncCommand(ExecutePickReferenceTaskCommand);
        async Task ExecutePickReferenceTaskCommand()
        {
            var param = await dialogService.ShowDialogAsync(DialogRoutes.MultiSelectTask, new DialogParameters { { NavigationKey.ReferenceTasks, Tasks }, { NavigationKey.MyTasks, myTasks } });
            if (param?.Parameters?.ContainsKey(NavigationKey.ReferenceTasks) == true)
            {
                Tasks = new List<TaskModel>(param.Parameters.GetValue<List<TaskModel>>(NavigationKey.ReferenceTasks));
            }
        }
    }
}
