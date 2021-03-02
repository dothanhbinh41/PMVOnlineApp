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

namespace PMVOnline.Homes.ViewModels
{
    public class HomeViewModel : TabViewModelBase
    {
        public List<TaskActionModel> Actions { get; set; }

        readonly INavigationService navigationService;
        public HomeViewModel(INavigationService navigationService)
        {
            Actions = new List<TaskActionModel> {
                new TaskActionModel{ User = "Do THanh Binh", Action = "Phe Duyet", Task = new TaskModel{ DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.High,Title="Mua vang" } },
                new TaskActionModel{ User = "Do THanh Binh", Action = "Phe Duyet", Task = new TaskModel{ DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.Normal,Title="Mua vang" }},
                new TaskActionModel{ User = "Do THanh Binh", Action = "Phe Duyet", Task = new TaskModel{ DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.Highest,Title="Mua vang" }},
            };
            this.navigationService = navigationService;
        }

        ICommand _ViewDetailCommand;
        public ICommand ViewDetailCommand => _ViewDetailCommand = _ViewDetailCommand ?? new AsyncCommand<TaskActionModel>(ExecuteViewDetailCommand);
        async Task ExecuteViewDetailCommand(TaskActionModel task)
        {
            await navigationService.NavigateAsync(Routes.TaskDetail);
        } 
    }
}
