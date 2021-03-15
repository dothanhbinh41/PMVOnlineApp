using PMVOnline.Common.Bases;
using PMVOnline.Tasks.Models;
using PMVOnline.Tasks.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PMVOnline.Tasks.ViewModels
{
    public class HistoryViewModel : ViewModelBase
    {
        private readonly ITaskService taskService;

        public List<HistoryModel> Histories { get; set; }
        public HistoryViewModel(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            LoadData(parameters.GetValue<long>(NavigationKey.TaskId));
        }

        async Task LoadData(long id)
        {
            IsBusy = true;
            var his = await taskService.GetTaskHistoryAsync(id);
            if (his?.Length > 0)
            {
                Histories = new List<HistoryModel>(his);
            }
            IsBusy = false;
        }
    }
}
