using PMVOnline.Common.Bases;
using PMVOnline.Common.Services;
using PMVOnline.Tasks.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PMVOnline.Tasks.ViewModels
{
    public class CompleteTaskViewModel : ViewModelBase
    {
        public string Note { get; set; }
        public DateTime? Date { get; set; }
        long taskId;
        private readonly INavigationService navigationService;
        private readonly ITaskService taskService;
        private readonly IDateTimeService dateTimeService;

        public CompleteTaskViewModel(INavigationService navigationService,
            ITaskService taskService,
             IDateTimeService dateTimeService)
        {
            this.navigationService = navigationService;
            this.taskService = taskService;
            this.dateTimeService = dateTimeService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            taskId = parameters.GetValue<long>(NavigationKey.TaskId);
        }

        ICommand _ChooseDueDateCommand;
        public ICommand ChooseDueDateCommand => _ChooseDueDateCommand = _ChooseDueDateCommand ?? new AsyncCommand(ExecuteChooseDueDateCommand);
        async Task ExecuteChooseDueDateCommand()
        {
            var date = await dateTimeService.PickDateTimeAsync(DateTime.Now, Date);
            if (date.HasValue)
            {
                Date = date.Value;
            }
        }


        ICommand _FinishCommand;
        public ICommand FinishCommand => _FinishCommand = _FinishCommand ?? new AsyncCommand<bool>(ExecuteFinishCommand);
        async Task ExecuteFinishCommand(bool completed)
        {
            if (!completed && string.IsNullOrEmpty(Note))
            {
                Toast("Bạn cần điền lý do");
                return;
            }

            IsBusy = true;
            var result = await taskService.CompleteTaskAsync(taskId, completed, Date ?? DateTime.Now, Note);
            IsBusy = false;
            if (result)
            {
                Toast("Hoàn thành");
                await navigationService.GoBackAsync(new NavigationParameters { { NavigationKey.Reload, true } });
            }
            else
            {
                Toast("Có lỗi xảy ra");
            }
        }
    }
}
