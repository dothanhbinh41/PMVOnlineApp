using PMVOnline.Common.Bases;
using PMVOnline.Homes.Models;
using PMVOnline.Tasks.Models;
using Prism.Navigation;
using System;
using System.Collections.Generic;
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

        public TaskViewModel(INavigationService navigationService)
        {
            Tasks = new List<TaskActionModel> {
                new TaskActionModel{ User = "Do THanh Binh", Action = "Phe Duyet", Task = new TaskModel{ DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.High } },
                new TaskActionModel{ User = "Do THanh Binh", Action = "Phe Duyet",  Task = new TaskModel{ DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.Normal }},
                new TaskActionModel{ User = "Do THanh Binh", Action = "Phe Duyet",  Task = new TaskModel{ DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.Highest }},
            };
            this.navigationService = navigationService;
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
            var xx = await navigationService.NavigateAsync(Routes.TaskDetail);
        }

    }
}
