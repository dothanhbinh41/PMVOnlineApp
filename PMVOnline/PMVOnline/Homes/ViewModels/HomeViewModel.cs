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
        public List<HotTaskModel> Tasks { get; set; }
         
        readonly INavigationService navigationService;
        public HomeViewModel(INavigationService navigationService)
        {
            Tasks = new List<HotTaskModel> {
                new HotTaskModel{ Assignee = "Do THanh Binh", Action = "Phe Duyet", DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.High,Title="Mua vang"},
                new HotTaskModel{ Assignee = "Do THanh Binh", Action = "Phe Duyet", DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.Normal,Title="Mua vang"},
                new HotTaskModel{ Assignee = "Do THanh Binh", Action = "Phe Duyet", DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.Highest,Title="Mua vang"},
            };
            this.navigationService = navigationService;
        }

        ICommand _CreateCommand;

        public ICommand CreateCommand => _CreateCommand = _CreateCommand ?? new AsyncCommand(ExcuteCreateCommand);
        async Task ExcuteCreateCommand()
        {
            await navigationService.NavigateAsync(Routes.CreateTask);
        } 
    }
}
