using PMVOnline.Common.Bases;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PMVOnline.Tasks.ViewModels.Dialogs
{
    public class WriteNoteViewModel : DialogViewModelBase
    {
        public override event Action<IDialogParameters> RequestClose;
        public string Note { get; set; }

        ICommand _CloseCommand;
        public ICommand CloseCommand => _CloseCommand = _CloseCommand ?? new Command(ExecuteCloseCommand);
        void ExecuteCloseCommand()
        {
            RequestClose?.Invoke(new DialogParameters());
        }

        ICommand _FinishCommand;
        public ICommand FinishCommand => _FinishCommand = _FinishCommand ?? new Command(ExecuteFinishCommand);
        void ExecuteFinishCommand()
        {
            RequestClose?.Invoke(new DialogParameters { { NavigationKey.Note, Note } });
        }
    }
}
