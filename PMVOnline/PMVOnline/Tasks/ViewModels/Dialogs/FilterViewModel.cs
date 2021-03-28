using PMVOnline.Accounts.Models;
using PMVOnline.Common.Bases;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PMVOnline.Tasks.ViewModels.Dialogs
{
    public class FilterViewModel : DialogViewModelBase
    {
        public override event Action<IDialogParameters> RequestClose;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<UserModel> Users { get; set; }
        public FilterViewModel()
        {
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
        }





    }
}
