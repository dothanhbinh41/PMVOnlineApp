using PMVOnline.Common.Bases;
using PMVOnline.Tasks.Models;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
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
        public MultiSelectTaskViewModel()
        {
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            var myTasks = parameters.GetValue<List<TaskModel>>(NavigationKey.MyTasks) ?? new List<TaskModel>();
            var tasks = parameters.GetValue<long[]>(NavigationKey.ReferenceTasks) ?? new long[0];
            Tasks = myTasks.Select(d => new TaskReferenceModel { Task = d, IsSelected = tasks.Any(c => c == d.Id) }).ToList();
        }

        ICommand _SelectCommand;
        public ICommand SelectCommand => _SelectCommand = _SelectCommand ?? new Command<TaskReferenceModel>(ExecuteSelectCommand);
        void ExecuteSelectCommand(TaskReferenceModel obj)
        {
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
    }
}
