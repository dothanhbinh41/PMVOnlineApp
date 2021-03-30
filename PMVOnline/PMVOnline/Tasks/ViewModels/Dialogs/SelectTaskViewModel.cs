using Acr.UserDialogs;
using PMVOnline.Common.Bases;
using PMVOnline.Tasks.Models;
using PMVOnline.Tasks.Services;
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
    public class SelectTaskViewModel : DialogViewModelBase
    {
        public override event Action<IDialogParameters> RequestClose;
        public List<TaskReferenceModel> Tasks { get; set; }
        public string Id { get; set; }
        public long? CorrectId { set; get; }


        readonly ITaskService taskService;


        public SelectTaskViewModel(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        List<TaskReferenceModel> sourceTasks;
        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            var myTasks = parameters.GetValue<List<TaskModel>>(NavigationKey.MyTasks) ?? new List<TaskModel>();
            var taskId = parameters.GetValue<TaskModel>(NavigationKey.CloneTask) ?? new TaskModel();
            sourceTasks = myTasks.Select(d => new TaskReferenceModel { Task = d, IsSelected = taskId.Id == d.Id }).ToList();
            Tasks = sourceTasks;
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



        ICommand _SearchCommand;
        public ICommand SearchCommand => _SearchCommand = _SearchCommand ?? new AsyncCommand(ExecuteSearchCommand);
        async Task ExecuteSearchCommand()
        {
            if (!CorrectId.HasValue)
            {
                return;
            }
            var local = Tasks?.FirstOrDefault(d => d.Task.Id == CorrectId.Value);

            if (local == null)
            {
                UserDialogs.Instance.ShowLoading();
                var t = await taskService.GetTaskAsync(CorrectId.Value);
                UserDialogs.Instance.HideLoading();
                if (t != null)
                {
                    Tasks = new List<TaskReferenceModel> { new TaskReferenceModel { IsSelected = false, Task = t } };
                }
            }
            else
            { 
                Tasks = new List<TaskReferenceModel> { local };
            }
        }


        public void OnIdChanged()
        {
            long id = 0;
            var p = long.TryParse(Id, out id);
            if (p)
            {
                CorrectId = id;
            }
            else
            {
                CorrectId = null;
                var local = sourceTasks.FirstOrDefault(d => d.IsSelected); 
                Tasks = sourceTasks;
            }
        }
    }
}
