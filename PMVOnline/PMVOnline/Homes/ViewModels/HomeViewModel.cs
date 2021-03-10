using PMVOnline.Common.Bases;
using PMVOnline.Homes.Models;
using PMVOnline.Tasks.Models;
using PMVOnline.Tasks.Services;
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
        private readonly ITaskService taskService;

        public HomeViewModel(INavigationService navigationService, ITaskService taskService)
        {
            this.navigationService = navigationService;
            this.taskService = taskService;
        }


        public override void RaiseIsActiveChanged()
        {
            base.RaiseIsActiveChanged();
            if (!IsActive)
            {
                return;
            }
            taskService.GetMyActionsAsync().ContinueWith(d => Actions = new List<TaskActionModel>(d.Result));
        }

        ICommand _ViewDetailCommand;
        public ICommand ViewDetailCommand => _ViewDetailCommand = _ViewDetailCommand ?? new AsyncCommand<TaskActionModel>(ExecuteViewDetailCommand);
        async Task ExecuteViewDetailCommand(TaskActionModel task)
        {
            await navigationService.NavigateAsync(Routes.TaskDetail);
        }
    }
}
