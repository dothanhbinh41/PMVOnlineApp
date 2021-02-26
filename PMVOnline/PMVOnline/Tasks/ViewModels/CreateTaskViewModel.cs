using System;
using System.Threading.Tasks;
using System.Windows.Input;
using PMVOnline.Common.Bases;
using PMVOnline.Tasks.Models;
using Prism.Navigation;
using Prism.Services.Dialogs;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PMVOnline.Tasks.ViewModels
{
    public class CreateTaskViewModel : ViewModelBase
    {

        public CreateTaskModel Task { get; set; } = new CreateTaskModel();

        readonly INavigationService navigationService;
        readonly IDialogService dialogService;

        public CreateTaskViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
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

    }
}