using PMVOnline.Common.Bases;
using PMVOnline.Common.Extensions;
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
    public class TargetModel : ModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public bool IsSelected { get; set; }
    }

    public class ChooseTargetViewModel : DialogViewModelBase
    { 
        public override event Action<IDialogParameters> RequestClose;
        public List<TargetModel> Targets { get; set; }

        TargetModel currentTarget; 

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            if (parameters.ContainsKey(NavigationKey.Target))
            {
                Targets = parameters.GetValue<List<TargetModel>>(NavigationKey.AllTargets); 
                currentTarget = parameters.GetValue<TargetModel>(NavigationKey.Target);
                if (currentTarget!=null)
                {
                    Targets.FirstOrDefault(d => d.Id == currentTarget.Id).IsSelected = true;
                }
            }
        }

        ICommand _SelectCommand;
        public ICommand SelectCommand => _SelectCommand = _SelectCommand ?? new Command<TargetModel>(ExecuteSelectCommand);
        void ExecuteSelectCommand(TargetModel obj)
        {
            if (currentTarget?.Id == obj.Id)
            {
                RequestClose?.Invoke(new DialogParameters { { NavigationKey.Target, currentTarget } });
            }
            else
            {
                RequestClose?.Invoke(new DialogParameters { { NavigationKey.Target, obj } });
            }
        }

        ICommand _CloseCommand;
        public ICommand CloseCommand => _CloseCommand = _CloseCommand ?? new Command(ExecuteCloseCommand);


        void ExecuteCloseCommand()
        {
            RequestClose?.Invoke(new DialogParameters { { NavigationKey.Target, currentTarget } });
        }
    }
}
