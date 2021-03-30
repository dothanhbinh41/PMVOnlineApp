using PMVOnline.Common.Bases;
using PMVOnline.Tasks.Models;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace PMVOnline.Tasks.ViewModels
{
    public class TaskReferenceModel : ModelBase
    {
        public TaskModel Task { get; set; }
        public bool IsSelected { get; set; }
    }

    public class MultiSelectTaskViewModel : DialogViewModelBase
    {
        public override event Action<IDialogParameters> RequestClose;
        public List<TaskReferenceModel> Tasks { get; set; }
        public bool Editable { get; set; }

        readonly IDialogService dialogService;
        public MultiSelectTaskViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            var myTasks = parameters.GetValue<List<TaskModel>>(NavigationKey.MyTasks) ?? new List<TaskModel>();
            Editable = parameters.GetValue<bool>(NavigationKey.Editable);
            var tasks = parameters.GetValue<List<TaskModel>>(NavigationKey.ReferenceTasks) ?? new List<TaskModel>();
            Tasks = myTasks.Select(d => new TaskReferenceModel { Task = d, IsSelected = tasks.Any(c => c.Id == d.Id) }).ToList();
        }

        ICommand _SelectCommand;
        public ICommand SelectCommand => _SelectCommand = _SelectCommand ?? new Command<TaskReferenceModel>(ExecuteSelectCommand);
        void ExecuteSelectCommand(TaskReferenceModel obj)
        {
            if (Editable)
                obj.IsSelected = !obj.IsSelected;
        }

        ICommand _CloseCommand;
        public ICommand CloseCommand => _CloseCommand = _CloseCommand ?? new Command(ExecuteCloseCommand);
        void ExecuteCloseCommand()
        {
            RequestClose?.Invoke(new DialogParameters());
        }

        ICommand _CompleteCommand;
        public ICommand CompleteCommand => _CompleteCommand = _CompleteCommand ?? new Command(ExecuteCompleteCommand);
        void ExecuteCompleteCommand()
        {
            RequestClose?.Invoke(new DialogParameters { { NavigationKey.ReferenceTasks, Tasks.Where(d => d.IsSelected).Select(d => d.Task).ToList() } });
        }


        ICommand _OpenDetailCommand;
        public ICommand OpenDetailCommand => _OpenDetailCommand = _OpenDetailCommand ?? new AsyncCommand<TaskReferenceModel>(ExecuteOpenDetailCommand);
        async Task ExecuteOpenDetailCommand(TaskReferenceModel task)
        {
            await dialogService.ShowDialogAsync(DialogRoutes.TaskDetail, new DialogParameters { { NavigationKey.TaskId, task.Task.Id } });
        }

    }
}
