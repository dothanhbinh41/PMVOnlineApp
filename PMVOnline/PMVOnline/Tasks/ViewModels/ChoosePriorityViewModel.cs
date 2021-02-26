using PMVOnline.Common.Bases;
using PMVOnline.Common.Extensions;
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
    public class PriorityModel : ModelBase
    {
        public string Text { get; set; }
        public TaskPriority Priority { get; set; }
        public bool IsSelected { get; set; }
    }
    public class ChoosePriorityViewModel : DialogViewModelBase
    {
        public override event Action<IDialogParameters> RequestClose;
        public List<PriorityModel> Priorities { get; set; }

        TaskPriority currentPriority;
        public ChoosePriorityViewModel()
        {
            Priorities = new List<PriorityModel>
            {
                new PriorityModel
                {
                    Text = TaskPriority.Normal.PriorityToString(),
                    Priority = TaskPriority.Normal
                },
                new PriorityModel
                {
                    Text = TaskPriority.High.PriorityToString(),
                    Priority = TaskPriority.High
                },
                new PriorityModel
                {
                    Text =TaskPriority.Highest.PriorityToString(),
                    Priority = TaskPriority.Highest
                }
            };
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            if (parameters.ContainsKey(NavigationKey.Priority))
            {
                currentPriority = parameters.GetValue<TaskPriority>(NavigationKey.Priority); 
                Priorities.FirstOrDefault(d => d.Priority == currentPriority).IsSelected = true;
            }
        }

        ICommand _SelectCommand;
        public ICommand SelectCommand => _SelectCommand = _SelectCommand ?? new Command<PriorityModel>(ExecuteSelectCommand);
        void ExecuteSelectCommand(PriorityModel obj)
        {
            if (currentPriority == obj.Priority)
            {
                RequestClose?.Invoke(new DialogParameters { { NavigationKey.Priority, currentPriority } });
            }
            else
            {
                RequestClose?.Invoke(new DialogParameters { { NavigationKey.Priority, obj.Priority } });
            }
        }

        ICommand _CloseCommand;
        public ICommand CloseCommand => _CloseCommand = _CloseCommand ?? new Command(ExecuteCloseCommand);
        void ExecuteCloseCommand()
        {
            RequestClose?.Invoke(new DialogParameters { { NavigationKey.Priority, currentPriority } });
        }
    }
}
