using PMVOnline.Common.Bases;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PMVOnline.Tasks.ViewModels.Dialogs
{
    public class RatingViewModel : DialogViewModelBase
    {
        public override event Action<IDialogParameters> RequestClose;
        public List<TaskReferenceModel> Tasks { get; set; }
        public long taskId { get; set; }

        public int Rating { get; set; }
        public RatingViewModel()
        {
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            taskId = parameters.GetValue<long>(NavigationKey.TaskId);
        }

        ICommand _SelectCommand;
        public ICommand SelectCommand => _SelectCommand = _SelectCommand ?? new Command<TaskReferenceModel>(ExecuteSelectCommand);
        void ExecuteSelectCommand(TaskReferenceModel obj)
        {
            RequestClose?.Invoke(new DialogParameters { { NavigationKey.CloneTask, obj.Task } });
        }
         
        ICommand _RateCommand;
        public ICommand RateCommand => _RateCommand = _RateCommand ?? new Command(ExecuteRateCommand);
        void ExecuteRateCommand()
        {
            RequestClose?.Invoke(new DialogParameters { { NavigationKey.Rating, Rating } });
        }


        ICommand _CloseCommand;
        public ICommand CloseCommand => _CloseCommand = _CloseCommand ?? new Command(ExecuteCloseCommand);
        void ExecuteCloseCommand()
        {
            RequestClose?.Invoke(new DialogParameters());
        }
    }
}
