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
    public class SelectTaskViewModel : DialogViewModelBase
    {
        public override event Action<IDialogParameters> RequestClose;
        public List<TaskReferenceModel> Tasks { get; set; }
        public SelectTaskViewModel()
        {
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            var myTasks = parameters.GetValue<List<TaskBaseModel>>(NavigationKey.MyTasks) ?? new List<TaskBaseModel>();
            var taskId = parameters.GetValue<TaskBaseModel>(NavigationKey.CloneTask) ?? new TaskBaseModel();
            Tasks = myTasks.Select(d => new TaskReferenceModel { Task = d, IsSelected = taskId.Id == d.Id }).ToList();
        }

        ICommand _SelectCommand;
        public ICommand SelectCommand => _SelectCommand = _SelectCommand ?? new Command<TaskReferenceModel>(ExecuteSelectCommand);
        void ExecuteSelectCommand(TaskReferenceModel obj)
        {
            RequestClose?.Invoke(new DialogParameters { { NavigationKey.CloneTask, obj.Task } });
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
            RequestClose?.Invoke(new DialogParameters { { NavigationKey.CloneTask, Tasks.Where(d => d.IsSelected).Select(d => d.Task).FirstOrDefault() } });
        }
    }
}
